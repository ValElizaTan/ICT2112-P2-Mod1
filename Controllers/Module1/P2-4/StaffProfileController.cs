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

    // ── Index / View Profile ─────────────────────────────────────────────

    [HttpGet]
    public IActionResult Index()
    {
        return ViewProfile(1);
    }

    public IActionResult ViewProfile(int staffId)
    {
        // ── Viewed staff ─────────────────────────────────────────────────
        Staff staff = _control.GetStaffInformation(staffId);
        var info = staff.GetStaffInfo();
        ViewBag.StaffInfo = new
        {
            StaffId      = info.StaffId,
            Role         = info.User.UserRole,
            Name         = info.User.Name,
            Email        = info.User.Email,
            PhoneCountry = info.User.PhoneCountry,
            PhoneNumber  = info.User.PhoneNumber
        };

        // ── All staff for the management table ───────────────────────────
        // FindAll() SELECT has no JOIN on User, so Staff.User is null for those rows.
        // Guard with s.User != null before touching GetStaffInfo() at all.
        var allStaff = new List<dynamic>();
        foreach (var s in _control.GetStaff())
        {
            if (s.User == null) continue;
            var i = s.GetStaffInfo();
            allStaff.Add((dynamic)new
            {
                StaffId      = i.StaffId,
                Role         = i.User.UserRole.ToString(),
                Name         = i.User.Name,
                Email        = i.User.Email,
                PhoneCountry = i.User.PhoneCountry,
                PhoneNumber  = i.User.PhoneNumber
            });
        }
        ViewBag.AllStaff = allStaff;

        return View("Index");
    }

    // ── Update ───────────────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
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

    // ── Create ───────────────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateStaff(int staffId, string name, string email,
        int phoneCountry, string phoneNumber, string passwordHash)
    {
        if (staffId <= 0)
            TempData["CrudError"] = "Staff ID must be a positive number.";
        else if (string.IsNullOrWhiteSpace(name))
            TempData["CrudError"] = "Name cannot be empty.";
        else if (string.IsNullOrWhiteSpace(email))
            TempData["CrudError"] = "Email cannot be empty.";
        else if (!email.Contains("@"))
            TempData["CrudError"] = "Email is not valid.";
        else if (phoneCountry <= 0)
            TempData["CrudError"] = "Phone country code is not valid.";
        else if (string.IsNullOrWhiteSpace(phoneNumber))
            TempData["CrudError"] = "Phone number cannot be empty.";
        else if (string.IsNullOrWhiteSpace(passwordHash))
            TempData["CrudError"] = "Password cannot be empty.";
        else
        {
            try
            {
                if (!int.TryParse(phoneNumber.Replace(" ", ""), out int phoneInt))
                    phoneInt = 0;

                bool created = _control.CreateStaff(staffId, name, email, phoneCountry,
                                                     phoneInt, passwordHash);
                TempData[created ? "CrudSuccess" : "CrudError"] = created
                    ? $"Staff member '{name}' (#{staffId}) created successfully."
                    : $"A staff member with ID #{staffId} already exists.";
            }
            catch (Exception ex)
            {
                TempData["CrudError"] = $"Failed to create staff: {ex.Message}";
            }
        }

        int currentId = TempData.Peek("CurrentStaffId") as int? ?? 1;
        return ViewProfile(currentId);
    }

    // ── Delete ───────────────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteStaff(int staffId)
    {
        try
        {
            _control.DeleteStaff(staffId);
            TempData["CrudSuccess"] = $"Staff member #{staffId} has been deleted.";
        }
        catch (Exception ex)
        {
            TempData["CrudError"] = $"Failed to delete staff: {ex.Message}";
        }

        int currentId = TempData.Peek("CurrentStaffId") as int? ?? 1;
        return ViewProfile(currentId);
    }
}
