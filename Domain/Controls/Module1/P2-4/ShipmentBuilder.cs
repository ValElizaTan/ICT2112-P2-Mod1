using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Controls.Module1.P2_4;

public class ShipmentBuilder : IShipmentBuilder
{
    private int _trackingId;
    private List<Order> _orders = new List<Order>();
    private string _destinationAddress = string.Empty;
    private bool _dispatchStatus;
    private int _batchId;
    private double _weight;

    public IShipmentBuilder BuildTrackingInfo(int id, double weight)
    {
        _trackingId = id;
        _weight = weight;
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
        var shipment = new Shipment(_trackingId, _weight, _destinationAddress, _dispatchStatus, _batchId);
        shipment.SetOrders(_trackingId);
        return shipment;
    }
}
