using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Entities;

/// <summary>
/// CarrierAgent represents a shipping/delivery partner.
/// This is a domain entity used by the Shipping module.
/// </summary>
public class CarrierAgent
{
    private readonly int _carrierId;
    private readonly string _name;
    private bool _isActive;
    private int _capacity;
    private readonly List<DeliveryType> _servicesAvailable;

    public CarrierAgent(int carrierId, string name, bool isActive, int capacity, List<DeliveryType> servicesAvailable)
    {
        _carrierId = carrierId;
        _name = name;
        _isActive = isActive;
        _capacity = capacity;
        _servicesAvailable = servicesAvailable ?? new List<DeliveryType>();
    }

    public int GetCarrierId() => _carrierId;
    public string GetName() => _name;
    public bool IsActive() => _isActive;
    public int GetCapacity() => _capacity;
    public List<DeliveryType> GetServicesAvailable() => new List<DeliveryType>(_servicesAvailable);

    public void SetActive(bool isActive) => _isActive = isActive;
    public void SetCapacity(int capacity) => _capacity = capacity;

    public bool CanServe(DeliveryType service) => _servicesAvailable.Contains(service);
    public bool HasCapacity(int additionalWeight) => _capacity >= additionalWeight;
}
