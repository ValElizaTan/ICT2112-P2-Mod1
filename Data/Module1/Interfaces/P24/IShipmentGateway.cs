using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Interfaces.P24;

/// <summary>
/// IShipmentGateway defines the data access contract for Shipment objects.
/// All shipment database operations must go through this interface.
/// </summary>
public interface IShipmentGateway
{
    Shipment? FindByTrackingId(int trackingId);
    List<Shipment> FindByOrderId(int orderId);
    List<Shipment> FindByCarrierId(int carrierId);
    List<Shipment> FindByStatus(ShipmentStatus status);
    List<Shipment> FindByFilter(ShipmentFilter filter);
    void Insert(Shipment shipment);
    void Update(Shipment shipment);
    void Delete(int trackingId);
}
