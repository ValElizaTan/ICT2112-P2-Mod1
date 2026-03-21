using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Interfaces.P24;

/// <summary>
/// IShipmentStatusHistoryGateway defines the data access contract for ShipmentStatusHistory objects.
/// All shipment status history database operations must go through this interface.
/// </summary>
public interface IShipmentStatusHistoryGateway
{
    ShipmentStatusHistory? FindById(int historyId);
    List<ShipmentStatusHistory> FindByTrackingId(int trackingId);
    List<ShipmentStatusHistory> FindByTrackingIdOrderedByTime(int trackingId);
    void Insert(ShipmentStatusHistory history);
    void DeleteByTrackingId(int trackingId);
}
