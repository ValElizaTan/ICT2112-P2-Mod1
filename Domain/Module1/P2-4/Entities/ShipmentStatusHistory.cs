using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Entities;

/// <summary>
/// ShipmentStatusHistory tracks the status changes of a shipment over time.
/// This is an audit trail for shipment status updates.
/// </summary>
public class ShipmentStatusHistory
{
    private readonly int _historyId;
    private readonly int _trackingId;
    private readonly ShipmentStatus _status;
    private readonly DateTime _timestamp;
    private readonly int _updatedBy;
    private readonly string? _remark;

    public ShipmentStatusHistory(int historyId, int trackingId, ShipmentStatus status,
                                DateTime timestamp, int updatedBy, string? remark = null)
    {
        _historyId = historyId;
        _trackingId = trackingId;
        _status = status;
        _timestamp = timestamp;
        _updatedBy = updatedBy;
        _remark = remark;
    }

    // Getters
    public int GetHistoryId() => _historyId;
    public int GetTrackingId() => _trackingId;
    public ShipmentStatus GetStatus() => _status;
    public DateTime GetTimestamp() => _timestamp;
    public int GetUpdatedBy() => _updatedBy;
    public string? GetRemark() => _remark;
}
