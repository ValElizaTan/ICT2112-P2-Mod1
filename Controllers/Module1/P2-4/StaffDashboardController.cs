using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

public class StaffDashboardController : Controller
{
    private readonly StaffDashboardControl _control;
    private readonly IOrderTrackingService _orderTrackingService;

    public StaffDashboardController(StaffDashboardControl control, IOrderTrackingService orderTrackingService)
    {
        _control = control;
        _orderTrackingService = orderTrackingService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var orders = _orderTrackingService.GetAllOrders() ?? new List<Order>();
        // var orders = _control.DisplayOrderList();
        var readyItems = _control.GetInventoryItemsByStatus(InventoryStatus.AVAILABLE);

        ViewBag.Orders = orders;
        ViewBag.InventoryItems = readyItems;

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
        return RedirectToAction("DisplayShipmentList", "Shipping");
    }
    public IActionResult OnNavigateToStaffProfile()
    {
        return RedirectToAction("Index", "StaffProfile");
    }

    public IActionResult OnLogout()
    {
        // Session clearing will be handled by ISessionService (Team 6)
        return RedirectToAction("Index", "Home");
    }
}
