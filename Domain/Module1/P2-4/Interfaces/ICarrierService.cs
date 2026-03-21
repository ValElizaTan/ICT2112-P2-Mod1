using ProRental.Domain.Module1.P24.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

/// <summary>
/// ICarrierService defines the contract for carrier agent management.
/// Segregated from IShippingService to follow Interface Segregation Principle.
/// </summary>
public interface ICarrierService
{
    CarrierAgent? GetCarrierById(int carrierId);
    List<CarrierAgent> GetAllCarriers();
    List<CarrierAgent> GetActiveCarriers();
    void RegisterCarrier(CarrierAgent agent);
    void UpdateCarrierStatus(int carrierId, bool isActive);
    void UpdateCarrierCapacity(int carrierId, int newCapacity);
    bool CanCarrierAccommodate(int carrierId, double weight);
}
