using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

/// <summary>
/// IShippingService defines the contract for shipping operations and tracking.
/// Controllers consume this interface, never the concrete ShipmentControl class directly.
/// </summary>
public interface IShippingService
{
    P24.Entities.Shipment CreateShipment(int orderId, double weight, string destinationAddress,
                           int carrierId, ShipmentPriority priority, DateTime estimatedArrival);
    P24.Entities.Shipment? GetShipmentByTrackingId(int trackingId);
    List<P24.Entities.Shipment> GetShipmentsByOrderId(int orderId);
    List<P24.Entities.Shipment> GetShipmentsByCarrierId(int carrierId);
    List<P24.Entities.Shipment> GetActiveShipments();
    List<P24.Entities.Shipment> SearchShipments(ShipmentFilter filter);
    void UpdateShipmentStatus(int trackingId, ShipmentStatus newStatus, string? remark = null);
    void AssignCarrierToShipment(int trackingId, int carrierId);
    List<ShipmentStatusHistory> GetShipmentTimeline(int trackingId);
}
