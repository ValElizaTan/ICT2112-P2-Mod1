using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

/// <summary>
/// CustomerShippingController is a <<boundary>> class for customer-facing shipping operations.
/// Customers can track their shipments, view delivery details, and initiate returns.
/// Delegates business logic to ShipmentControl via IShippingService.
/// </summary>
[Route("Module1/CustomerShipping")]
[ApiController]
public class CustomerShippingController : Controller
{
    private readonly IShippingService _shippingService;
    private readonly ICarrierService _carrierService;

    public CustomerShippingController(IShippingService shippingService, ICarrierService carrierService)
    {
        _shippingService = shippingService ?? throw new ArgumentNullException(nameof(shippingService));
        _carrierService = carrierService ?? throw new ArgumentNullException(nameof(carrierService));
    }

    /// <summary>
    /// Renders the customer shipping dashboard.
    /// Displays shipment details, tracking info, and ETA.
    /// </summary>
    [HttpGet("Dashboard/{trackingId}")]
    public IActionResult Dashboard(int trackingId)
    {
        var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
        if (shipment == null)
            return NotFound();

        var carrier = _carrierService.GetCarrierById(shipment.GetCarrierId());
        var timeline = _shippingService.GetShipmentTimeline(trackingId);

        ViewData["Shipment"] = shipment;
        ViewData["Carrier"] = carrier;
        ViewData["Timeline"] = timeline;
        ViewData["LastUpdated"] = DateTime.UtcNow;
        ViewData["Title"] = $"Track Shipment {trackingId}";

        return View("~/Views/Module1/P2-4/Shipping/CustomerDashboard.cshtml");
    }

    /// <summary>
    /// Get shipment details via API (returns JSON).
    /// Used for AJAX requests from the customer dashboard.
    /// </summary>
    [HttpGet("Shipment/{trackingId}")]
    public IActionResult GetShipment(int trackingId)
    {
        var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
        if (shipment == null)
            return NotFound(new { error = "Shipment not found" });

        var carrier = _carrierService.GetCarrierById(shipment.GetCarrierId());

        return Json(new
        {
            success = true,
            data = new
            {
                trackingId = shipment.GetTrackingId(),
                orderId = shipment.GetOrderId(),
                weight = shipment.GetWeight(),
                destination = shipment.GetDestinationAddress(),
                status = shipment.GetStatus().ToString(),
                priority = shipment.GetPriority().ToString(),
                estimatedArrival = shipment.GetEstimatedArrival(),
                carrier = carrier != null ? new
                {
                    id = carrier.GetCarrierId(),
                    name = carrier.GetName(),
                    isActive = carrier.IsActive(),
                    capacity = carrier.GetCapacity()
                } : null,
                lastUpdated = DateTime.UtcNow
            }
        });
    }

    /// <summary>
    /// Get the complete shipment status timeline.
    /// This shows the customer all status changes with timestamps and remarks.
    /// </summary>
    [HttpGet("Shipment/{trackingId}/Timeline")]
    public IActionResult GetShipmentTimeline(int trackingId)
    {
        var timeline = _shippingService.GetShipmentTimeline(trackingId);
        
        var timelineData = timeline.Select(h => new
        {
            status = h.GetStatus().ToString(),
            timestamp = h.GetTimestamp(),
            remark = h.GetRemark()
        }).ToList();

        return Json(new { success = true, data = timelineData });
    }

    /// <summary>
    /// Refresh the shipment status and return latest info.
    /// Shows the customer when the dashboard was last updated.
    /// </summary>
    [HttpPost("Shipment/{trackingId}/Refresh")]
    public IActionResult RefreshShipment(int trackingId)
    {
        try
        {
            var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
            if (shipment == null)
                return NotFound(new { error = "Shipment not found" });

            return Json(new
            {
                success = true,
                data = new
                {
                    trackingId = shipment.GetTrackingId(),
                    status = shipment.GetStatus().ToString(),
                    estimatedArrival = shipment.GetEstimatedArrival(),
                    lastUpdated = DateTime.UtcNow
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Initiate a return process for an item in this shipment.
    /// Delegates to Module2's return flow (not implemented here).
    /// </summary>
    [HttpPost("Shipment/{trackingId}/InitiateReturn")]
    public IActionResult InitiateReturn(int trackingId, [FromBody] InitiateReturnRequest request)
    {
        try
        {
            // TODO: Call Module2.IReturnService to initiate return process
            // For now, just acknowledge the request
            return Ok(new
            {
                success = true,
                message = "Return process initiated. You will receive a return label via email.",
                returnTrackingId = new Random().Next(100000, 999999)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Get carrier contact details for this shipment.
    /// Customers can use this to contact the carrier directly.
    /// </summary>
    [HttpGet("Shipment/{trackingId}/CarrierContact")]
    public IActionResult GetCarrierContact(int trackingId)
    {
        var shipment = _shippingService.GetShipmentByTrackingId(trackingId);
        if (shipment == null)
            return NotFound(new { error = "Shipment not found" });

        var carrier = _carrierService.GetCarrierById(shipment.GetCarrierId());
        if (carrier == null)
            return NotFound(new { error = "Carrier information not available" });

        // TODO: In production, fetch actual contact details from database
        return Json(new
        {
            success = true,
            data = new
            {
                carrierName = carrier.GetName(),
                phone = "1-800-CARRIER", // Placeholder
                email = "support@carrier.com", // Placeholder
                website = "https://www.carrier.com" // Placeholder
            }
        });
    }
}

/// <summary>
/// Request model for initiating a return.
/// </summary>
public class InitiateReturnRequest
{
    public int? ItemId { get; set; }
    public string? Reason { get; set; }
}
