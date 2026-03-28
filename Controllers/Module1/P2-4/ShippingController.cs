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

public IActionResult DisplayShipmentList(int? trackingId = null)
{
    if (trackingId.HasValue)
    {
        _control.LoadShipment(trackingId.Value);
        var single = _control.GetShipment();
        var list = single != null ? new List<Shipment> { single } : new List<Shipment>();
        ViewBag.FilteredId = trackingId.Value;
        return View("~/Views/Module1/P2-4/Shipping/ShippingDashboard.cshtml", list);
    }

    return View("~/Views/Module1/P2-4/Shipping/ShippingDashboard.cshtml", _control.GetAllShipments());
}

public IActionResult ShowCarrierPerformance()
{
    if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

    var shipments = _control.GetAllShipments();
    var dispatched = shipments.Count(s => s.GetShipmentInfo().DispatchStatus);
    var pending    = shipments.Count - dispatched;

    ViewBag.TotalShipments  = shipments.Count;
    ViewBag.DispatchedCount = dispatched;
    ViewBag.PendingCount    = pending;
    ViewBag.DispatchRate    = shipments.Count > 0
        ? Math.Round((double)dispatched / shipments.Count * 100, 1)
        : 0.0;

    return View("~/Views/Module1/P2-4/Shipping/ShippingDashboard.cshtml", shipments);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult UpdateManualStatus(int trackingId, bool dispatchStatus)
{
    if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

    var existing = _control.GetAllShipments()
        .FirstOrDefault(s => s.GetShipmentInfo().TrackingId == trackingId);

    if (existing != null)
    {
        var info = existing.GetShipmentInfo();
        info.DispatchStatus = dispatchStatus;
        existing.SetShipmentInfo(info);
    }

    return RedirectToAction(nameof(DisplayShipmentList));
}
}