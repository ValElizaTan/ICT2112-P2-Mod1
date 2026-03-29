using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class ShipmentBuilder : IShipmentBuilder
{
    private int _trackingId;
    private List<Order> _orders = new List<Order>();
    private string _destinationAddress = string.Empty;
    private bool _dispatchStatus;
    private int _batchId;
    private double _weight = 1.0;

    public IShipmentBuilder BuildTrackingInfo(int id, double weight)
    {
        _trackingId = id;
        _weight = weight > 0 ? weight : 1.0;
        return this;
    }

    public IShipmentBuilder BuildAddressInfo(string addr)
    {
        _destinationAddress = addr;
        return this;
    }

    public IShipmentBuilder BuildDispatchInfo(bool status, int batchId)
    {
        _dispatchStatus = status;
        _batchId = batchId;
        return this;
    }

    public Shipment Build()
    {
        return new Shipment(_trackingId, _weight, _destinationAddress, _dispatchStatus, _batchId);
    }
}
