using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class StaffProfileController : Controller
{
    private readonly StaffControl _control;

    public StaffProfileController(StaffControl control)
    {
        _control = control;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return ViewProfile(1);
    }

    public IActionResult ViewProfile(int staffId)
    {
        Staff staff = _control.GetStaffInformation(staffId);
        var info = staff.GetStaffInfo();
        ViewBag.StaffInfo = new
        {
            StaffId = info.StaffId,
            Role = info.User.UserRole,
            Name = info.User.Name,
            Email = info.User.Email,
            PhoneCountry = info.User.PhoneCountry,
            PhoneNumber = info.User.PhoneNumber
        };
        return View("Index");

    }
    [HttpPost]
    public IActionResult UpdateStaffDetails(int staffId, string name, string email,
    int phoneCountry, string phoneNumber, string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
            TempData["ErrorMessage"] = "Name cannot be empty.";
        else if (string.IsNullOrWhiteSpace(email))
            TempData["ErrorMessage"] = "Email cannot be empty.";
        else if (!email.Contains("@"))
            TempData["ErrorMessage"] = "Email is not valid.";
        else if (phoneCountry <= 0)
            TempData["ErrorMessage"] = "Phone country code is not valid.";
        else if (string.IsNullOrWhiteSpace(phoneNumber))
            TempData["ErrorMessage"] = "Phone number cannot be empty.";
        else
        {
            try
            {
                _control.UpdateStaff(staffId, name, email, phoneCountry, phoneNumber, passwordHash);
                TempData["SuccessMessage"] = "Profile updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to update profile: {ex.Message}";
            }
        }

        return ViewProfile(staffId);
    }
}
