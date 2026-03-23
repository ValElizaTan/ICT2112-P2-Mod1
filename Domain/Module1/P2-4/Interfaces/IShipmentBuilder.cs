using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface IShipmentBuilder
{
    IShipmentBuilder BuildTrackingInfo(int id, double weight);
    IShipmentBuilder BuildAddressInfo(string addr);
    IShipmentBuilder BuildDispatchInfo(bool status, int batchId);
    Shipment Build();
}
