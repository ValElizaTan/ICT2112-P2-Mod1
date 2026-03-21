using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

/// <summary>
/// IShipmentBuilder defines the contract for building Shipment objects step-by-step.
/// Uses the Builder pattern to ensure complex Shipment objects are constructed properly.
/// </summary>
public interface IShipmentBuilder
{
    IShipmentBuilder BuildTrackingInfo(int trackingId, int orderId, double weight);
    IShipmentBuilder BuildAddressInfo(string destinationAddress);
    IShipmentBuilder BuildDispatchInfo(bool dispatchStatus, int batchId);
    IShipmentBuilder BuildCarrierInfo(int carrierId, ShipmentPriority priority);
    IShipmentBuilder BuildDeliveryInfo(ShipmentStatus status, DateTime estimatedArrival);
    P24.Entities.Shipment Build();
}
