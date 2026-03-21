using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Entities;

/// <summary>
/// Shipment represents a physical shipment/delivery of an order.
/// Always constructed via ShipmentBuilder - never instantiate directly.
/// </summary>
public class Shipment
{
    private readonly int _trackingId;
    private readonly int _orderId;
    private readonly double _weight;
    private readonly string _destinationAddress;
    private readonly bool _dispatchStatus;
    private readonly int _batchId;
    private readonly int _carrierId;
    private readonly ShipmentPriority _priority;
    private ShipmentStatus _status;
    private DateTime _estimatedArrival;

    // Constructor - package visibility, called only by ShipmentBuilder
    internal Shipment(int trackingId, int orderId, double weight, string destinationAddress,
                     bool dispatchStatus, int batchId, int carrierId, ShipmentPriority priority,
                     ShipmentStatus status, DateTime estimatedArrival)
    {
        _trackingId = trackingId;
        _orderId = orderId;
        _weight = weight;
        _destinationAddress = destinationAddress;
        _dispatchStatus = dispatchStatus;
        _batchId = batchId;
        _carrierId = carrierId;
        _priority = priority;
        _status = status;
        _estimatedArrival = estimatedArrival;
    }

    // Getters
    public int GetTrackingId() => _trackingId;
    public int GetOrderId() => _orderId;
    public double GetWeight() => _weight;
    public string GetDestinationAddress() => _destinationAddress;
    public bool IsDispatched() => _dispatchStatus;
    public int GetBatchId() => _batchId;
    public int GetCarrierId() => _carrierId;
    public ShipmentPriority GetPriority() => _priority;
    public ShipmentStatus GetStatus() => _status;
    public DateTime GetEstimatedArrival() => _estimatedArrival;

    // Setters
    public void SetStatus(ShipmentStatus status) => _status = status;
    public void SetEstimatedArrival(DateTime estimatedArrival) => _estimatedArrival = estimatedArrival;
}

/// <summary>
/// Represents the priority level for a shipment.
/// </summary>
public enum ShipmentPriority
{
    Low,
    Medium,
    High,
    Urgent
}
