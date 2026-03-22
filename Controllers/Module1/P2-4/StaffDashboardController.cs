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

    [HttpGet]
    public IActionResult Index()
    {
        var orders = _control.DisplayOrderList();
        var readyItems = _control.GetInventoryItemsByStatus(InventoryStatus.AVAILABLE);

        ViewData["Orders"] = orders;
        ViewData["InventoryItems"] = readyItems;

        return View();
    }

    

    public IActionResult OnNavigateToWalkIn(string type = "new")
    {
        if (type == "existing")
            return RedirectToAction("Index", "WalkInOrder");

        return RedirectToAction("EnterCustomerDetails", "WalkInOrder");
    }

    public IActionResult OnNavigateToShipping()
    {
        return RedirectToAction("Index", "Shipping");
    }

    public IActionResult OnLogout()
    {
        // Session clearing will be handled by ISessionService (Team 6)
        return RedirectToAction("Index", "Home");
    }
}
