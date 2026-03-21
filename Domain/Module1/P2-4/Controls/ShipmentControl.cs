using ProRental.Data.Module1.Interfaces.P24;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

/// <summary>
/// ShipmentControl is the core business logic controller for shipment operations.
/// It orchestrates shipment creation, tracking, and status management.
/// Implements IShippingService to expose its operations to controllers/other modules.
/// </summary>
public class ShipmentControl : IShippingService
{
    private readonly IShipmentGateway _shipmentGateway;
    private readonly IShipmentStatusHistoryGateway _historyGateway;
    private readonly IShipmentBuilder _builder;

    public ShipmentControl(
        IShipmentGateway shipmentGateway,
        IShipmentStatusHistoryGateway historyGateway,
        IShipmentBuilder builder)
    {
        _shipmentGateway = shipmentGateway ?? throw new ArgumentNullException(nameof(shipmentGateway));
        _historyGateway = historyGateway ?? throw new ArgumentNullException(nameof(historyGateway));
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    /// <summary>
    /// Creates a new shipment using the builder pattern.
    /// Never instantiate Shipment directly - always use the builder.
    /// </summary>
    public P24.Entities.Shipment CreateShipment(int orderId, double weight, string destinationAddress,
                                  int carrierId, ShipmentPriority priority, DateTime estimatedArrival)
    {
        ValidateShipmentInput(orderId, weight, destinationAddress, carrierId, estimatedArrival);

        var shipment = _builder
            .BuildTrackingInfo(GenerateTrackingId(), orderId, weight)
            .BuildAddressInfo(destinationAddress)
            .BuildDispatchInfo(false, 0)
            .BuildCarrierInfo(carrierId, priority)
            .BuildDeliveryInfo(ShipmentStatus.PENDING, estimatedArrival)
            .Build();

        _shipmentGateway.Insert(shipment);
        RecordStatusHistory(shipment.GetTrackingId(), ShipmentStatus.PENDING, null, "Shipment created");

        return shipment;
    }

    public P24.Entities.Shipment? GetShipmentByTrackingId(int trackingId)
    {
        return _shipmentGateway.FindByTrackingId(trackingId);
    }

    public List<P24.Entities.Shipment> GetShipmentsByOrderId(int orderId)
    {
        return _shipmentGateway.FindByOrderId(orderId);
    }

    public List<P24.Entities.Shipment> GetShipmentsByCarrierId(int carrierId)
    {
        return _shipmentGateway.FindByCarrierId(carrierId);
    }

    public List<P24.Entities.Shipment> GetActiveShipments()
    {
        var active = new List<ShipmentStatus> 
        { 
            ShipmentStatus.PENDING, 
            ShipmentStatus.IN_TRANSIT 
        };

        return active.SelectMany(status => _shipmentGateway.FindByStatus(status)).ToList();
    }

    public List<P24.Entities.Shipment> SearchShipments(ShipmentFilter filter)
    {
        return _shipmentGateway.FindByFilter(filter);
    }

    public void UpdateShipmentStatus(int trackingId, ShipmentStatus newStatus, string? remark = null)
    {
        var shipment = GetShipmentByTrackingId(trackingId);
        if (shipment is null)
            throw new InvalidOperationException($"Shipment with tracking ID {trackingId} not found");

        shipment.SetStatus(newStatus);
        _shipmentGateway.Update(shipment);
        RecordStatusHistory(trackingId, newStatus, null, remark);
    }

    public void AssignCarrierToShipment(int trackingId, int carrierId)
    {
        var shipment = GetShipmentByTrackingId(trackingId);
        if (shipment is null)
            throw new InvalidOperationException($"Shipment with tracking ID {trackingId} not found");

        // In a real scenario, you'd rebuild the shipment with new carrier info
        // For now, we just update the existing one
        _shipmentGateway.Update(shipment);
        RecordStatusHistory(trackingId, shipment.GetStatus(), null, $"Carrier reassigned to ID {carrierId}");
    }

    public List<ShipmentStatusHistory> GetShipmentTimeline(int trackingId)
    {
        return _historyGateway.FindByTrackingIdOrderedByTime(trackingId);
    }

    /// <summary>
    /// Records a status change in the shipment history audit trail.
    /// </summary>
    private void RecordStatusHistory(int trackingId, ShipmentStatus status, int? updatedBy, string? remark)
    {
        var history = new ShipmentStatusHistory(
            GenerateHistoryId(),
            trackingId,
            status,
            DateTime.UtcNow,
            updatedBy ?? 0,
            remark
        );

        _historyGateway.Insert(history);
    }

    private void ValidateShipmentInput(int orderId, double weight, string destinationAddress,
                                       int carrierId, DateTime estimatedArrival)
    {
        if (orderId <= 0)
            throw new ArgumentException("Order ID must be positive", nameof(orderId));

        if (weight <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weight));

        if (string.IsNullOrWhiteSpace(destinationAddress))
            throw new ArgumentException("Destination address cannot be empty", nameof(destinationAddress));

        if (carrierId <= 0)
            throw new ArgumentException("Carrier ID must be positive", nameof(carrierId));

        if (estimatedArrival <= DateTime.Now)
            throw new ArgumentException("Estimated arrival must be in the future", nameof(estimatedArrival));
    }

    private int GenerateTrackingId()
    {
        // In production, this would come from database sequence/identity
        return new Random().Next(100000, 999999);
    }

    private int GenerateHistoryId()
    {
        // In production, this would come from database sequence/identity
        return new Random().Next(1000000, 9999999);
    }
}
