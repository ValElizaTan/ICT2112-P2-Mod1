using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface INotificationSubject
{
    void RegisterObserver(INotificationObserver observer);
    void UnregisterObserver(INotificationObserver observer);
    void NotifyObservers(Notification notification);

    bool CreateNotification(int userId, string message, Enums.NotificationType type);
    List<Notification> GetUserNotifications(int userId);
    bool MarkAsRead(int notificationId);

    ProRental.Domain.Entities.NotificationInfo? GetPendingPopup(int userId);
    void ClearPendingPopup(int userId);
}
