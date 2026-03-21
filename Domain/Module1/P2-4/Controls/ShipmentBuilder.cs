using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

/// <summary>
/// ShipmentBuilder implements IShipmentBuilder and follows the Builder design pattern.
/// Ensures Shipment objects are always constructed with all required information.
/// </summary>
public class ShipmentBuilder : IShipmentBuilder
{
    private int _trackingId;
    private int _orderId;
    private double _weight;
    private string? _destinationAddress;
    private bool _dispatchStatus;
    private int _batchId;
    private int _carrierId;
    private ShipmentPriority _priority;
    private ShipmentStatus _status;
    private DateTime _estimatedArrival;

    public IShipmentBuilder BuildTrackingInfo(int trackingId, int orderId, double weight)
    {
        _trackingId = trackingId;
        _orderId = orderId;
        _weight = weight;
        return this;
    }

    public IShipmentBuilder BuildAddressInfo(string destinationAddress)
    {
        _destinationAddress = destinationAddress ?? throw new ArgumentNullException(nameof(destinationAddress));
        return this;
    }

    public IShipmentBuilder BuildDispatchInfo(bool dispatchStatus, int batchId)
    {
        _dispatchStatus = dispatchStatus;
        _batchId = batchId;
        return this;
    }

    public IShipmentBuilder BuildCarrierInfo(int carrierId, ShipmentPriority priority)
    {
        _carrierId = carrierId;
        _priority = priority;
        return this;
    }

    public IShipmentBuilder BuildDeliveryInfo(ShipmentStatus status, DateTime estimatedArrival)
    {
        _status = status;
        _estimatedArrival = estimatedArrival;
        return this;
    }

    public P24.Entities.Shipment Build()
    {
        ValidateBuiltObject();
        
        return new P24.Entities.Shipment(
            _trackingId,
            _orderId,
            _weight,
            _destinationAddress!,
            _dispatchStatus,
            _batchId,
            _carrierId,
            _priority,
            _status,
            _estimatedArrival
        );
    }

    private void ValidateBuiltObject()
    {
        if (_orderId <= 0)
            throw new InvalidOperationException("Order ID must be set and greater than 0");
        
        if (_weight <= 0)
            throw new InvalidOperationException("Weight must be set and greater than 0");
        
        if (string.IsNullOrWhiteSpace(_destinationAddress))
            throw new InvalidOperationException("Destination address must be set");
        
        if (_carrierId <= 0)
            throw new InvalidOperationException("Carrier ID must be set and greater than 0");
        
        if (_estimatedArrival <= DateTime.Now)
            throw new InvalidOperationException("Estimated arrival must be in the future");
    }
}
