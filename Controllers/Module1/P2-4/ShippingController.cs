using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

/// <summary>
/// ShippingController is a <<boundary>> class that handles staff shipping operations.
/// Exposes staff dashboard, shipment management, and carrier assignment features.
/// Delegates all business logic to ShipmentControl and CarrierControl via their interfaces.
/// </summary>
[Route("Module1/Shipping")]
[ApiController]
public class ShippingController : Controller
{
    private readonly IShippingService _shippingService;
    private readonly ICarrierService _carrierService;

    public ShippingController(IShippingService shippingService, ICarrierService carrierService)
    {
        _shippingService = shippingService ?? throw new ArgumentNullException(nameof(shippingService));
        _carrierService = carrierService ?? throw new ArgumentNullException(nameof(carrierService));
    }

    /// <summary>
    /// Renders the staff shipping dashboard.
    /// Displays carrier agents, active shipments, and filtering options.
    /// </summary>
    [HttpGet("Dashboard")]
    public IActionResult Dashboard()
    {
        var activeShipments = _shippingService.GetActiveShipments();
        var carriers = _carrierService.GetAllCarriers();

        ViewData["ActiveShipments"] = activeShipments;
        ViewData["Carriers"] = carriers;
        ViewData["Title"] = "Shipping Dashboard";

        return View("~/Views/Module1/P2-4/Shipping/Dashboard.cshtml");
    }

    /// <summary>
    /// Search and filter shipments based on criteria.
    /// Used by staff to find specific shipments.
    /// </summary>
    [HttpPost("Search")]
    public IActionResult Search([FromBody] ShipmentFilterRequest request)
    {
        var filter = new ShipmentFilter(
            request.FromDate,
            request.ToDate,
            request.CarrierId,
            request.Status,
            request.Route
        );

        var results = _shippingService.SearchShipments(filter);
        return Json(new { success = true, data = results });
    }

    /// <summary>
    /// Get details of a specific shipment.
    /// </summary>
    [HttpGet("Shipment/{trackingId}")]
    public IActionResult GetShipment(int trackingId)
    {
        var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
        if (shipment == null)
            return NotFound(new { error = "Shipment not found" });

        return Json(new { success = true, data = shipment });
    }

    /// <summary>
    /// Update the status of a shipment (e.g., mark as in transit, delivered).
    /// </summary>
    [HttpPut("Shipment/{trackingId}/Status")]
    public IActionResult UpdateShipmentStatus(int trackingId, [FromBody] UpdateStatusRequest request)
    {
        try
        {
            _shippingService.UpdateShipmentStatus(trackingId, request.Status, request.Remark);
            return Ok(new { success = true, message = "Shipment status updated" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Assign or reassign a carrier to a shipment.
    /// Staff can use this to balance workload across carriers.
    /// </summary>
    [HttpPut("Shipment/{trackingId}/AssignCarrier")]
    public IActionResult AssignCarrier(int trackingId, [FromBody] AssignCarrierRequest request)
    {
        try
        {
            var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
            if (shipment == null)
                return NotFound(new { error = "Shipment not found" });

            var carrier = _carrierService.GetCarrierById(request.CarrierId);
            if (carrier == null)
                return BadRequest(new { error = "Carrier not found" });

            if (!carrier.IsActive())
                return BadRequest(new { error = "Carrier is not active" });

            _shippingService.AssignCarrierToShipment(trackingId, request.CarrierId);
            return Ok(new { success = true, message = "Carrier assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Get all active carriers with their current status and capacity.
    /// </summary>
    [HttpGet("Carriers")]
    public IActionResult GetCarriers()
    {
        var carriers = _carrierService.GetAllCarriers();
        return Json(new { success = true, data = carriers });
    }

    /// <summary>
    /// Get shipments assigned to a specific carrier.
    /// </summary>
    [HttpGet("Carrier/{carrierId}/Shipments")]
    public IActionResult GetCarrierShipments(int carrierId)
    {
        var shipments = _shippingService.GetShipmentsByCarrierId(carrierId);
        return Json(new { success = true, data = shipments });
    }
}

/// <summary>
/// Request model for filtering shipments.
/// </summary>
public class ShipmentFilterRequest
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CarrierId { get; set; }
    public ShipmentStatus? Status { get; set; }
    public string? Route { get; set; }
}

/// <summary>
/// Request model for updating shipment status.
/// </summary>
public class UpdateStatusRequest
{
    public ShipmentStatus Status { get; set; }
    public string? Remark { get; set; }
}

/// <summary>
/// Request model for assigning a carrier to a shipment.
/// </summary>
public class AssignCarrierRequest
{
    public int CarrierId { get; set; }
}
