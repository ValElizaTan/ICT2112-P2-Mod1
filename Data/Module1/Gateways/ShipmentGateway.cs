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

    public Shipment? FindByShipmentId(int shipmentId)
    {
        return _context.Shipments.Find(shipmentId);
    }

    public void InsertShipment(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        _context.SaveChanges();
    }

    public void Update(int shipmentId, ShipmentData data)
    {
        var existing = _context.Shipments.Find(shipmentId);
        if (existing == null) return;

        existing.SetTrackingId(data.TrackingId);
        existing.SetDispatchStatus(data.DispatchStatus);
        existing.SetDestinationAddress(data.DestinationAddress);
        existing.SetBatchId(data.BatchId);

        _context.Shipments.Update(existing);
        _context.SaveChanges();
    }

    public void Delete(int shipmentId)
    {
        var existing = _context.Shipments.Find(shipmentId);
        if (existing == null) return;

        _context.Shipments.Remove(existing);
        _context.SaveChanges();
    }
}
