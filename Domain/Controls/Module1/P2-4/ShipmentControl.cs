using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;
using Domain.Entities.Module1.P2_4.Placeholders;


namespace ProRental.Domain.Controls.Module1.P2_4;

public class ShipmentControl
{
    private readonly IShipmentBuilder _builder;
    private Shipment? _currentShipment;

    public ShipmentControl(IShipmentBuilder builder)
    {
        _builder = builder;
    }

    public bool CreateShipment(int trackingId, double weight, string destinationAddress, bool dispatchStatus, int batchId)
    {
        if (!ValidateAddress(destinationAddress))
        {
            return false;
        }

        _currentShipment = _builder
            .BuildTrackingInfo(trackingId, weight)
            .BuildAddressInfo(destinationAddress)
            .BuildDispatchInfo(dispatchStatus, batchId)
            .Build();

        return _currentShipment != null;
    }

    public bool ProcessShippingOrder()
    {
        if (_currentShipment == null) return false;

        if (!_currentShipment.IsDispatched())
        {
            _currentShipment.UpdateDispatchStatus(true);
            return true;
        }

        return false;
    }

    public Shipment? GetShipment() => _currentShipment;

    public DeliveryMethod GetDeliveryMethod() => new DeliveryMethod();

    public decimal CalculateRates(double weight)
    {
        const decimal baseRate = 10.00m;
        const decimal perKg = 2.50m;

        return baseRate + (decimal)weight * perKg;
    }

    public bool GetDispatchStatus() => _currentShipment?.IsDispatched() ?? false;

    private bool ValidateAddress(string destinationAddress)
    {
        return !string.IsNullOrWhiteSpace(destinationAddress) && destinationAddress.Length >= 5;
    }

    public int GetBatchId(int orderId, string destinationAddress)
    {
        return Math.Abs((orderId + destinationAddress.GetHashCode()) % 1000);
    }
}
