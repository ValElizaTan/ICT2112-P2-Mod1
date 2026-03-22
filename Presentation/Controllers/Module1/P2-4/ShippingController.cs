using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls.Module1.P2_4;
using ProRental.Domain.Entities;

namespace ProRental.Controllers.Module1.P2_4;

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
