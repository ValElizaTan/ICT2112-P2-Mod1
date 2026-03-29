using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

public class NotificationDisplayController : Controller, INotificationObserver
{
    private readonly INotificationSubject _subject;
    private readonly INotificationPreferenceService _preferenceService;

    private static readonly Dictionary<int, DateTime> _lastDailyPopup = new();
    private static readonly Dictionary<int, DateTime> _lastWeeklyPopup = new();

    public NotificationDisplayController(INotificationSubject subject, INotificationPreferenceService preferenceService)
    {
        _subject = subject;
        _preferenceService = preferenceService;
    }

    public void Subscribe()
    {
        _subject.RegisterObserver(this);
    }

    public void Unsubscribe()
    {
        _subject.UnregisterObserver(this);
    }

    public void Update(Notification notification)
    {
        // Observer callback (could push to UI, logs, SignalR etc.)
    }

    public IActionResult ShowNotifications(int userId)
    {
        var notifications = _subject.GetUserNotifications(userId);
        ViewData["UserId"] = userId;
        ViewData["Notifications"] = notifications;
        ViewData["LatestNotificationId"] = notifications.Any() ? notifications.Max(n => n.GetNotificationInfo().NotificationId) : 0;
        return View("~/Views/Module1/P2-4/NotificationDisplay/Index.cshtml");
    }

    [HttpGet]
    public IActionResult Index(int userId)
    {
        Subscribe();
        return ShowNotifications(userId);
    }

    [HttpGet]
    public JsonResult CheckPopup(int userId = 1)
    {
        var preference = _preferenceService.GetPreference(userId)?.GetNotificationPreferenceInfo();
        if (preference == null || preference.NotificationGranularity == NotificationGranularity.NONE)
            return Json(new { show = false });

        var pending = _subject.GetPendingPopup(userId);
        if (pending == null)
            return Json(new { show = false });

        if (preference.NotificationGranularity == NotificationGranularity.IMPORTANT_ONLY)
        {
            // TODO: apply actual importance filter; currently treat as eligible
        }

        // Keep pending until the user explicitly marks it as read; this lets all active browser tabs receive the same notification.
        return Json(new { show = true, id = pending.NotificationId, message = $"({pending.Type}) {pending.Message}" });
    }

    private static int GetWeekOfYear(DateTime date)
    {
        var dfi = System.Globalization.DateTimeFormatInfo.CurrentInfo;
        var cal = dfi.Calendar;
        return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
    }

    [HttpPost]
    public JsonResult AcknowledgePopup(int id, int userId = 1)
    {
        var success = _subject.MarkAsRead(id);
        _subject.ClearPendingPopup(userId);
        return Json(new { success, acknowledgedId = id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MarkAsRead(int id, int userId = 1)
    {
        _subject.MarkAsRead(id);
        _subject.ClearPendingPopup(userId);
        return RedirectToAction("Index", new { userId });
    }
}
