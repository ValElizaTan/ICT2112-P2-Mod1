using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface IShipmentGateway
{
    Shipment? FindByShipmentId(int shipmentId);
    void InsertShipment(Shipment shipment);
    void Update(int shipmentId, ShipmentData data);
    void Delete(int shipmentId);
    List<Shipment> GetShipments();
}
