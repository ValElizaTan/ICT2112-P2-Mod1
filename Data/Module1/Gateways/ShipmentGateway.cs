using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Gateways;

public class ShipmentGateway : IShipmentGateway
{
    private readonly AppDbContext _context;

    public ShipmentGateway(AppDbContext context)
    {
        _context = context;
    }

    // Private methods for internal data access
    private Shipment? GetShipmentById(int shipmentId)
    {
        return _context.Shipments.Find(shipmentId);
    }

    private void SaveChanges()
    {
        _context.SaveChanges();
    }

    // Public methods exposed at the bottom of the class
    public Shipment? FindByShipmentId(int shipmentId)
    {
        return GetShipmentById(shipmentId);
    }

    public void InsertShipment(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        SaveChanges();
    }

    public void Update(int shipmentId, ShipmentData data)
    {
        var existing = GetShipmentById(shipmentId);
        if (existing == null) return;

        existing.UpdateTrackingId(data.TrackingId);
        existing.UpdateDispatchStatus(data.DispatchStatus);
        existing.UpdateDestinationAddress(data.DestinationAddress);
        existing.UpdateBatchId(data.BatchId);

        _context.Shipments.Update(existing);
        SaveChanges();
    }

    public void Delete(int shipmentId)
    {
        var existing = GetShipmentById(shipmentId);
        if (existing == null) return;

        _context.Shipments.Remove(existing);
        SaveChanges();
    }
}
