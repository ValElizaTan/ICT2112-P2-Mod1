using ProRental.Data.Module1.Interfaces.P24;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Gateways.P24;

/// <summary>
/// ShipmentStatusHistoryGateway handles all database operations for ShipmentStatusHistory objects.
/// This is a TableDataGateway class implementing IShipmentStatusHistoryGateway.
/// </summary>
public class ShipmentStatusHistoryGateway : IShipmentStatusHistoryGateway
{
    private readonly AppDbContext _context;

    public ShipmentStatusHistoryGateway(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ShipmentStatusHistory? FindById(int historyId)
    {
        // TODO: Query history record by ID
        return null;
    }

    public List<ShipmentStatusHistory> FindByTrackingId(int trackingId)
    {
        // TODO: Query all history records for a shipment (tracking ID)
        return new List<ShipmentStatusHistory>();
    }

    public List<ShipmentStatusHistory> FindByTrackingIdOrderedByTime(int trackingId)
    {
        // TEMPORARY: Return test data for UI testing
        if (trackingId == 1)
        {
            return new List<ShipmentStatusHistory>
            {
                new ShipmentStatusHistory(1, 1, ShipmentStatus.PENDING, DateTime.Now.AddDays(-2), 0, "Shipment created"),
                new ShipmentStatusHistory(2, 1, ShipmentStatus.IN_TRANSIT, DateTime.Now.AddDays(-1), 1, "Picked up by carrier"),
                new ShipmentStatusHistory(3, 1, ShipmentStatus.IN_TRANSIT, DateTime.Now, 1, "In transit to destination")
            };
        }
        return new List<ShipmentStatusHistory>();
    }

    public void Insert(ShipmentStatusHistory history)
    {
        if (history == null)
            throw new ArgumentNullException(nameof(history));

        // TODO: Insert history record into database
        _context.SaveChanges();
    }

    public void DeleteByTrackingId(int trackingId)
    {
        // TODO: Delete all history records for a shipment
        _context.SaveChanges();
    }
}
