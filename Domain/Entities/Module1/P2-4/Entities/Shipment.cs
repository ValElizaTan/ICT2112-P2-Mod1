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

    private Shipment GetShipmentInfo() => this;

    private void SetShipmentInfo(int trackingId)
    {
        _trackingId = trackingId;
    }

    // Public methods exposed at the bottom of the class
    public int TrackingId() => GetTrackingId();

    public IReadOnlyList<Order> Orders() => GetOrders();

    public string DestinationAddress() => GetDestinationAddress();

    public bool IsDispatched() => GetDispatchStatus();

    public int BatchId() => GetBatchId();

    public Shipment ShipmentInfo() => GetShipmentInfo();

    public void UpdateTrackingId(int trackingId) => SetTrackingId(trackingId);

    public void UpdateOrders(int trackingId) => SetOrders(trackingId);

    public void UpdateDestinationAddress(string destinationAddress) => SetDestinationAddress(destinationAddress);

    public void UpdateDispatchStatus(bool dispatchStatus) => SetDispatchStatus(dispatchStatus);

    public void UpdateBatchId(int batchId) => SetBatchId(batchId);

    public void UpdateShipmentInfo(int trackingId) => SetShipmentInfo(trackingId);
}
