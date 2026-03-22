using System.Collections.Generic;

namespace ProRental.Domain.Entities;

public partial class Shipment
{
    private int _trackingId;
    private List<Order> _orders = new List<Order>();
    private string _destinationAddress = string.Empty;
    private bool _dispatchStatus;
    private int _batchId;

    public Shipment(int trackingId, double weight, string destinationAddress, bool dispatchStatus, int batchId)
    {
        _trackingId = trackingId;
        // Weight is stored in the scaffolded entity portion as _weight / Weight property.
        // Set it via setter in scaffold or via alternative method if available.
        _destinationAddress = destinationAddress;
        _dispatchStatus = dispatchStatus;
        _batchId = batchId;
    }

    public int GetTrackingId() => _trackingId;

    public void SetTrackingId(int trackingId)
    {
        _trackingId = trackingId;
    }

    public List<Order> GetOrders() => _orders;

    public void SetOrders(int trackingId)
    {
        // Ambiguous signature from spec; this method sets shipments' order list to empty based on trackingId.
        // In a real implementation, this should retrieve related orders by trackingId.
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

    public Shipment GetShipmentInfo() => this;

    public void SetShipmentInfo(int trackingId)
    {
        _trackingId = trackingId;
    }
}
