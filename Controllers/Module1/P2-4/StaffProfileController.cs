using Microsoft.AspNetCore.Mvc;
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

    public IActionResult UpdateStaffDetails(int staffId, string name, string email, int phoneCountry, int phoneNumber, string passwordHash)
    {
        // _control.UpdateStaff(staffId, name, email, phoneCountry, phoneNumber, passwordHash);
        return RedirectToAction("Index");
    }

    public IActionResult ViewProfile(int staffId)
    {
        // var staffInfo = _control.GetStaffInformation(staffId); // uncomment when ready
        ViewData["StaffInfo"] = new
        {
            StaffId = staffId,
            Role = UserRole.STAFF,
            Name = "John Doe",
            Email = "john.doe@example.com",
            PhoneCountry = 65,
            PhoneNumber = "912345d67"
        };
        return View("Index");
    }
}
