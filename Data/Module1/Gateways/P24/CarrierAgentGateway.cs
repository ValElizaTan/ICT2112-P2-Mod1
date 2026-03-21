using ProRental.Data.Module1.Interfaces.P24;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Gateways.P24;

/// <summary>
/// CarrierAgentGateway handles all database operations for CarrierAgent objects.
/// This is a TableDataGateway class implementing ICarrierAgentGateway.
/// </summary>
public class CarrierAgentGateway : ICarrierAgentGateway
{
    private readonly AppDbContext _context;

    public CarrierAgentGateway(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public CarrierAgent? FindById(int carrierId)
    {
        // TEMPORARY: Return test data for UI testing
        if (carrierId == 1)
        {
            return new CarrierAgent(1, "FastShip Express", true, 50000, new List<DeliveryType> { DeliveryType.STANDARD, DeliveryType.EXPRESS });
        }
        return null;
    }

    public List<CarrierAgent> FindAll()
    {
        // TEMPORARY: Return test data for UI testing
        return new List<CarrierAgent>
        {
            new CarrierAgent(1, "FastShip Express", true, 50000, new List<DeliveryType> { DeliveryType.STANDARD, DeliveryType.EXPRESS }),
            new CarrierAgent(2, "AirSpeed Logistics", true, 25000, new List<DeliveryType> { DeliveryType.EXPRESS }),
            new CarrierAgent(3, "GroundHaul Transport", false, 75000, new List<DeliveryType> { DeliveryType.STANDARD })
        };
    }

    public List<CarrierAgent> FindByActive(bool isActive)
    {
        // TODO: Filter carrier agents by active status
        return new List<CarrierAgent>();
    }

    public void Insert(CarrierAgent agent)
    {
        if (agent == null)
            throw new ArgumentNullException(nameof(agent));

        // TODO: Convert domain CarrierAgent to database entity and insert
        _context.SaveChanges();
    }

    public void Update(CarrierAgent agent)
    {
        if (agent == null)
            throw new ArgumentNullException(nameof(agent));

        // TODO: Convert domain CarrierAgent to database entity and update
        _context.SaveChanges();
    }

    public void Delete(int carrierId)
    {
        // TODO: Delete carrier agent by ID
        _context.SaveChanges();
    }
}
