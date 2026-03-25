using System;
using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module1.P24;

public class NotificationSettingsController : Controller
{
    private readonly INotificationPreferenceService _preferenceService;

    public NotificationSettingsController(INotificationPreferenceService preferenceService)
    {
        _preferenceService = preferenceService;
    }

    [HttpGet]
    public IActionResult Index(string role = "customer", int userId = 1)
    {
        if (string.Equals(role, "staff", StringComparison.OrdinalIgnoreCase))
            userId = 12;

        var preference = _preferenceService.GetPreference(userId);
        NotificationPreferenceInfo model;

        if (preference != null)
        {
            model = preference.GetNotificationPreferenceInfo();
        }
        else
        {
            model = new NotificationPreferenceInfo(
                0,
                userId,
                NotificationFrequency.DAILY,
                NotificationGranularity.ALL,
                true,
                false
            );
        }

        ViewData["UserRole"] = role;
        ViewData["UserId"] = userId;

        return View("~/Views/Module1/P2-4/NotificationSettings/Index.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save(NotificationPreferenceInfo model, string role = "customer")
    {
        if (model == null)
            return BadRequest();

        var preference = _preferenceService.GetPreference(model.UserId) ?? new Notificationpreference(
            0, model.UserId, model.NotificationFrequency, model.NotificationGranularity, 
            model.EmailEnabled, model.SmsEnabled);
        preference.SetNotificationPreferenceInfo(model);

        _preferenceService.SetPreference(preference);

        return RedirectToAction("Index", new { role = role, userId = model.UserId });
    }
}
