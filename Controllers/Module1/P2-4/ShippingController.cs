using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Entities;

namespace ProRental.Controllers.Module1.P24;

// <<boundary>>
public class ShippingController : Controller
{
    private readonly ShipmentControl _control;

    public ShippingController(ShipmentControl control)
    {
        _control = control;
    }

    private bool IsStaff()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "";
        return role.Equals("STAFF", StringComparison.OrdinalIgnoreCase)
            || role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
    }

    public IActionResult DisplayShipmentList()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        var shipment = _control.GetShipment();
        return View("ShippingDashboard", shipment);
    }

    public IActionResult ShowCarrierPerformance()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        return View("CarrierPerformance");
    }

    [HttpPost]
    public IActionResult UpdateManualStatus()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        _control.ProcessShippingOrder();
        return RedirectToAction("DisplayShipmentList");
    }
}
