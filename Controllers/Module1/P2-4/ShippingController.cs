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

    // Check if current user is staff for access control
    private bool IsStaff()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "";
        return role.Equals("STAFF", StringComparison.OrdinalIgnoreCase)
            || role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
    }

    // Display the shipment list, with optional filtering by tracking ID
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
    }