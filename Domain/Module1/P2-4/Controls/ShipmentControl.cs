using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class ShipmentControl
{
    private readonly IShipmentBuilder _builder;
    private readonly IShipmentGateway _shipmentGateway;
    private Shipment? _currentShipment = null;

    public ShipmentControl(IShipmentBuilder builder, IShipmentGateway shipmentGateway)
    {
        _builder = builder;
        _shipmentGateway = shipmentGateway;
    }

    public bool ProcessShippingOrder()
    {
        if (_currentShipment == null) return false;

        if (!_currentShipment.GetDispatchStatus())
        {
            _currentShipment.SetDispatchStatus(true);
            return true;
        }

        return false;
    }

    public Shipment GetShipment()
    {
        if (_currentShipment == null)
        {
            throw new InvalidOperationException("No shipment is currently loaded.");
        }

        return _currentShipment;
    }

    public bool GetDispatchStatus() => _currentShipment?.GetDispatchStatus() ?? false;

    public List<Shipment> GetAllShipments()
    {
        return _shipmentGateway.GetShipments();
    }

    public bool LoadShipment(int trackingId)
    {
        var shipment = _shipmentGateway.GetShipments()
                                       .FirstOrDefault(s => s.GetTrackingId() == trackingId);
        if (shipment == null) return false;
        _currentShipment = shipment;
        return true;
    }
}
