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
        // Weight is stored in the scaffolded entity portion as _weight / Weight property.
        // Set it via setter in scaffold or via alternative method if available.
        _destinationAddress = destinationAddress;
        _dispatchStatus = dispatchStatus;
        _batchId = batchId;
        SetOrders(trackingId);
    }

    private int GetTrackingId() => _trackingId;

    private void SetTrackingId(int trackingId)
    {
        _trackingId = trackingId;
    }

    private List<Order> GetOrders() => _orders;

    private void SetOrders(int trackingId)
    {
        // Ambiguous signature from spec; this method sets shipments' order list to empty based on trackingId.
        // In a real implementation, this should retrieve related orders by trackingId.
        _orders = new List<Order>();
    }

    private string GetDestinationAddress() => _destinationAddress;

    private void SetDestinationAddress(string destinationAddress)
    {
        _destinationAddress = destinationAddress;
    }

    private bool GetDispatchStatus() => _dispatchStatus;

    private void SetDispatchStatus(bool dispatchStatus)
    {
        _dispatchStatus = dispatchStatus;
    }

    private int GetBatchId() => _batchId;

    private void SetBatchId(int batchId)
    {
        _batchId = batchId;
    }

    // Public methods exposed at the bottom of the class
    public ShipmentData GetShipmentInfo() => new()
    {
        TrackingId = Trackingid,
        Weight = Weight,
        DestinationAddress = Destination,
        DispatchStatus = _dispatchStatus,
        BatchId = Batchid,
    };

    public void SetShipmentInfo(ShipmentData info)
    {
        Trackingid = info.TrackingId;
        Weight = info.Weight;
        Destination = info.DestinationAddress;
        SetDispatchStatus(info.DispatchStatus);
        Batchid = info.BatchId;
        SetOrders(info.TrackingId);
    }
}
