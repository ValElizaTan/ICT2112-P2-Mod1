using ProRental.Data.Module1.Interfaces.P24;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Gateways.P24;

/// <summary>
/// ShipmentGateway handles all database operations for Shipment objects.
/// This is a TableDataGateway class implementing IShipmentGateway.
/// </summary>
public class ShipmentGateway : IShipmentGateway
{
    private readonly AppDbContext _context;

    public ShipmentGateway(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Shipment? FindByTrackingId(int trackingId)
    {
        // TEMPORARY: Return test data for UI testing
        if (trackingId == 1)
        {
            return new Shipment(1, 123, 25.5, "123 Main St, San Francisco, CA", true, 0, 1, ShipmentPriority.High, ShipmentStatus.PENDING, DateTime.Now.AddDays(3));
        }
        return null;
    }

    public List<Shipment> FindByOrderId(int orderId)
    {
        // TODO: Query by order ID
        return new List<Shipment>();
    }

    public List<Shipment> FindByCarrierId(int carrierId)
    {
        // TODO: Query by carrier ID
        return new List<Shipment>();
    }

    public List<Shipment> FindByStatus(ShipmentStatus status)
    {
        // TODO: Query by status
        return new List<Shipment>();
    }

    public List<Shipment> FindByFilter(ShipmentFilter filter)
    {
        if (filter == null || filter.IsEmpty())
            return new List<Shipment>();

        // TODO: Apply filter logic
        // This would filter by date range, carrier, status, and route
        var query = new List<Shipment>();

        if (filter.GetCarrierId().HasValue)
        {
            var carrierId = filter.GetCarrierId();
            if (carrierId.HasValue)
            {
                query = FindByCarrierId(carrierId.Value);
            }
        }

        if (filter.GetStatus().HasValue)
        {
            query = query.Where(s => s.GetStatus() == filter.GetStatus()).ToList();
        }

        if (filter.GetFromDate().HasValue && filter.GetToDate().HasValue)
        {
            query = query.Where(s =>
                s.GetEstimatedArrival() >= filter.GetFromDate() &&
                s.GetEstimatedArrival() <= filter.GetToDate()
            ).ToList();
        }

        return query;
    }

    public void Insert(Shipment shipment)
    {
        if (shipment == null)
            throw new ArgumentNullException(nameof(shipment));

        // TODO: Convert domain Shipment to database entity and insert
        _context.SaveChanges();
    }

    public void Update(Shipment shipment)
    {
        if (shipment == null)
            throw new ArgumentNullException(nameof(shipment));

        // TODO: Convert domain Shipment to database entity and update
        _context.SaveChanges();
    }

    public void Delete(int trackingId)
    {
        // TODO: Delete shipment by tracking ID
        _context.SaveChanges();
    }
}
