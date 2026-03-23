using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class ShipmentControl
{
    private readonly IShipmentBuilder _builder;
    private Shipment? _currentShipment = null;

    public ShipmentControl(IShipmentBuilder builder)
    {
        _builder = builder;
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
}
