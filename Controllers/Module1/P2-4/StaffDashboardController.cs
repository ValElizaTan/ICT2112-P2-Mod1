using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class StaffDashboardController : Controller
{
    private readonly StaffDashboardControl _control;

    public StaffDashboardController(StaffDashboardControl control)
    {
        _control = control;
    }

    private bool IsStaff()
    {
        var role = HttpContext.Session.GetString("UserRole");
        return !string.IsNullOrEmpty(role) &&
               (role.Equals("STAFF", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        var orders = _control.DisplayOrderList();
        var readyItems = _control.GetInventoryItemsByStatus(InventoryStatus.AVAILABLE);

        ViewData["Orders"] = orders;
        ViewData["InventoryItems"] = readyItems;

        return View();
    }

    public IActionResult OnNavigateToWalkIn(string type = "new")
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        if (type == "existing")
            return RedirectToAction("SelectExistingCustomer", "WalkInOrder");

        return RedirectToAction("EnterCustomerDetails", "WalkInOrder");
    }

    public IActionResult OnNavigateToShipping()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        return RedirectToAction("DisplayShipmentList", "Shipping");
    }

    public IActionResult OnNavigateToStaffProfile()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        return RedirectToAction("Index", "StaffProfile");
    }

    public IActionResult OnLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
