using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Data.Module1.Interfaces.P24;

/// <summary>
/// ICarrierAgentGateway defines the data access contract for CarrierAgent objects.
/// All carrier agent database operations must go through this interface.
/// </summary>
public interface ICarrierAgentGateway
{
    CarrierAgent? FindById(int carrierId);
    List<CarrierAgent> FindAll();
    List<CarrierAgent> FindByActive(bool isActive);
    void Insert(CarrierAgent agent);
    void Update(CarrierAgent agent);
    void Delete(int carrierId);
}
