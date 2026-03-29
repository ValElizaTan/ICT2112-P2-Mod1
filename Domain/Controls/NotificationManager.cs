using System.Collections.Concurrent;
using System.Linq;
using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class NotificationManager : INotificationSubject
{
    private readonly INotificationGateway _notificationGateway;
    private readonly INotificationPreferenceService _preferenceService;
    private readonly List<INotificationObserver> _observers = new();
    private static readonly ConcurrentDictionary<int, NotificationInfo> _pendingPopups = new();
    private static readonly ConcurrentDictionary<int, DateTime> _lastDailyPopup = new();
    private static readonly ConcurrentDictionary<int, DateTime> _lastWeeklyPopup = new();

    public NotificationManager(INotificationGateway notificationGateway, INotificationPreferenceService preferenceService)
    {
        _notificationGateway = notificationGateway;
        _preferenceService = preferenceService;
    }

    public void RegisterObserver(INotificationObserver observer)
    {
        if (observer != null && !_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void UnregisterObserver(INotificationObserver observer)
    {
        if (observer != null)
            _observers.Remove(observer);
    }

    public void NotifyObservers(Notification notification)
    {
        foreach (var observer in _observers)
        {
            observer.Update(notification);
        }
    }

    public bool CreateNotification(int userId, string message, Enums.NotificationType type = Enums.NotificationType.SYSTEM)
    {
        try
        {
            var notification = new Notification();
            var info = new NotificationInfo(0, userId, message, type, false, DateTime.UtcNow);
            notification.SetNotificationInfo(info);

            _notificationGateway.InsertNotification(notification);
            NotifyObservers(notification);

            // Apply preference logic for pop-up scheduling
            var pref = _preferenceService.GetPreference(userId)?.GetNotificationPreferenceInfo();
            if (pref != null && pref.NotificationGranularity != Enums.NotificationGranularity.NONE)
            {
                bool allowedByFreq = pref.NotificationFrequency switch
                {
                    Enums.NotificationFrequency.INSTANT => true,
                    Enums.NotificationFrequency.DAILY => !_lastDailyPopup.TryGetValue(userId, out var daily) || daily.Date < DateTime.UtcNow.Date,
                    Enums.NotificationFrequency.WEEKLY => !_lastWeeklyPopup.TryGetValue(userId, out var weekly) || GetWeekOfYear(weekly) < GetWeekOfYear(DateTime.UtcNow),
                    _ => false
                };

                if (allowedByFreq)
                {
                    // refresh from saved record to get actual key value if DB generated it
                    var persistedNotification = _notificationGateway.FindById(notification.GetNotificationInfo().NotificationId);
                    var popupInfo = persistedNotification?.GetNotificationInfo() ?? notification.GetNotificationInfo();
                    _pendingPopups[userId] = popupInfo;

                    if (pref.NotificationFrequency == Enums.NotificationFrequency.DAILY)
                        _lastDailyPopup[userId] = DateTime.UtcNow;
                    if (pref.NotificationFrequency == Enums.NotificationFrequency.WEEKLY)
                        _lastWeeklyPopup[userId] = DateTime.UtcNow;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<Notification> GetUserNotifications(int userId)
    {
        return _notificationGateway.FindByUser(userId);
    }
    
    public NotificationInfo? GetPendingPopup(int userId)
    {
        if (_pendingPopups.TryGetValue(userId, out var value))
            return value;

        var earliestUnread = _notificationGateway.FindUnreadByUser(userId)
            .OrderBy(n => n.GetNotificationInfo().DateSent)
            .FirstOrDefault();

        if (earliestUnread != null)
        {
            var info = earliestUnread.GetNotificationInfo();
            _pendingPopups[userId] = info;
            return info;
        }

        return null;
    }

    public void ClearPendingPopup(int userId)
    {
        _pendingPopups.TryRemove(userId, out _);
    }

    public bool MarkAsRead(int notificationId)
    {
        try
        {
            var notification = _notificationGateway.FindById(notificationId);
            if (notification != null)
            {
                var isReadProp = notification.GetType().GetProperty("Isread", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (isReadProp != null)
                {
                    isReadProp.SetValue(notification, true);
                }
                _notificationGateway.UpdateNotification(notification);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    private static int GetWeekOfYear(DateTime date)
    {
        var dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo ?? System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;
        var cal = dfi.Calendar;
        return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    }

}

