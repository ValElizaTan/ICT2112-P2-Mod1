using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module1.P24;

[Route("QueueNotification")]
public class NotificationCreationController : Controller
{
    private readonly INotificationSubject _subject;

    public NotificationCreationController(INotificationSubject subject)
    {
        _subject = subject;
    }

    [HttpGet("")]
    public IActionResult Index(int userId = 1)
    {
        ViewData["UserId"] = userId;
        ViewData["NotificationTypes"] = Enum.GetValues(typeof(NotificationType)).Cast<NotificationType>().ToList();
        ViewData["SuccessMessage"] = TempData["SuccessMessage"] as string;

        return View("~/Views/Module1/P2-4/QueueNotification/Index.cshtml");
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(int userId, string message, NotificationType type)
    {
        if (string.IsNullOrWhiteSpace(message))
            ModelState.AddModelError("Message", "Message is required.");

        if (!ModelState.IsValid)
        {
            ViewData["UserId"] = userId;
            ViewData["NotificationTypes"] = Enum.GetValues(typeof(NotificationType)).Cast<NotificationType>().ToList();
            return View("~/Views/Module1/P2-4/QueueNotification/Index.cshtml");
        }

        var success = CreateNotification(userId, message, type);
        TempData["SuccessMessage"] = success ? $"Notification queued for user {userId} (type: {type})" : "Failed to queue notification";

        return RedirectToAction("Index", new { userId });
    }

    public bool CreateNotification(int userId, string message, NotificationType type)
    {
        return _subject.CreateNotification(userId, message, type);
    }
}

