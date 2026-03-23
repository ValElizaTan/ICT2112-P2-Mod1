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

    public IActionResult DisplayShipmentList()
    {
        var shipment = _control.GetShipment();
        return View("ShippingDashboard", shipment);
    }

    public IActionResult ShowCarrierPerformance()
    {
        return View("CarrierPerformance");
    }

    [HttpPost]
    public IActionResult UpdateManualStatus()
    {
        _control.ProcessShippingOrder();
        return RedirectToAction("DisplayShipmentList");
    }
}
