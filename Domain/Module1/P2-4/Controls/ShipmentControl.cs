using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
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

        var info = _currentShipment.GetShipmentInfo();
        if (!info.DispatchStatus)
        {
            info.DispatchStatus = true;
            _currentShipment.SetShipmentInfo(info);
            return true;
        }

        return false;
    }

    public Shipment? GetShipment() => _currentShipment;

    public bool GetDispatchStatus() => _currentShipment?.GetShipmentInfo().DispatchStatus ?? false;

    public List<Shipment> GetAllShipments()
    {
        return _shipmentGateway.GetShipments();
    }

    public bool LoadShipment(int trackingId)
    {
        var shipment = _shipmentGateway.GetShipments()
                                       .FirstOrDefault(s => s.GetShipmentInfo().TrackingId == trackingId);
        if (shipment == null) return false;
        _currentShipment = shipment;
        return true;
    }
}
