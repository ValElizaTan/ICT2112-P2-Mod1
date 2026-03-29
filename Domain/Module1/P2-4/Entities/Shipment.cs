using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public record ShipmentData
{
    public int TrackingId { get; set; }
    public double Weight { get; set; }
    public string DestinationAddress { get; set; } = string.Empty;
    public bool DispatchStatus { get; set; }
    public int BatchId { get; set; }
}

public partial class Shipment
{
    private int _trackingId;
    private List<Order> _orders = new List<Order>();
    private string _destinationAddress = string.Empty;
    private bool _dispatchStatus;
    private int _batchId;

    protected Shipment() { }

    public Shipment(int trackingId, double weight, string destinationAddress, bool dispatchStatus, int batchId)
    {
        _trackingId = trackingId;
        SetWeight(weight);
        _destinationAddress = destinationAddress;
        _dispatchStatus = dispatchStatus;
        _batchId = batchId;
        SetOrders(trackingId);
    }

    public int GetTrackingId() => _trackingId;

    public void SetTrackingId(int trackingId)
    {
        _trackingId = trackingId;
    }

    public List<Order> GetOrders() => _orders;

    public void SetOrders(int trackingId)
    {
        _orders = new List<Order>();
    }

    public string GetDestinationAddress() => _destinationAddress;

    public void SetDestinationAddress(string destinationAddress)
    {
        _destinationAddress = destinationAddress;
    }

    public bool GetDispatchStatus() => _dispatchStatus;

    public void SetDispatchStatus(bool dispatchStatus)
    {
        _dispatchStatus = dispatchStatus;
    }

    public int GetBatchId() => _batchId;

    public void SetBatchId(int batchId)
    {
        _batchId = batchId;
    }

    public double GetWeight() => Weight;

    public void SetWeight(double weight)
    {
        Weight = weight;
    }

    public Shipment GetShipmentInfo() => this;

    public void SetShipmentInfo(int trackingId)
    {
        SetTrackingId(trackingId);
        SetOrders(trackingId);
    }
}
