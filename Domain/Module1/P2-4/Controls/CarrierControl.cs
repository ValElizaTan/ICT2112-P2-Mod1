using ProRental.Data.Module1.Interfaces.P24;
using ProRental.Domain.Module1.P24.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

/// <summary>
/// CarrierControl manages all carrier agent operations.
/// Implements ICarrierService to expose operations to controllers.
/// </summary>
public class CarrierControl : ICarrierService
{
    private readonly ICarrierAgentGateway _carrierGateway;

    public CarrierControl(ICarrierAgentGateway carrierGateway)
    {
        _carrierGateway = carrierGateway ?? throw new ArgumentNullException(nameof(carrierGateway));
    }

    public CarrierAgent? GetCarrierById(int carrierId)
    {
        return _carrierGateway.FindById(carrierId);
    }

    public List<CarrierAgent> GetAllCarriers()
    {
        return _carrierGateway.FindAll();
    }

    public List<CarrierAgent> GetActiveCarriers()
    {
        return _carrierGateway.FindByActive(true);
    }

    public void RegisterCarrier(CarrierAgent agent)
    {
        if (agent == null)
            throw new ArgumentNullException(nameof(agent));

        ValidateCarrierAgent(agent);
        _carrierGateway.Insert(agent);
    }

    public void UpdateCarrierStatus(int carrierId, bool isActive)
    {
        var agent = GetCarrierById(carrierId);
        if (agent == null)
            throw new InvalidOperationException($"Carrier with ID {carrierId} not found");

        agent.SetActive(isActive);
        _carrierGateway.Update(agent);
    }

    public void UpdateCarrierCapacity(int carrierId, int newCapacity)
    {
        if (newCapacity <= 0)
            throw new ArgumentException("Capacity must be positive", nameof(newCapacity));

        var agent = GetCarrierById(carrierId);
        if (agent == null)
            throw new InvalidOperationException($"Carrier with ID {carrierId} not found");

        agent.SetCapacity(newCapacity);
        _carrierGateway.Update(agent);
    }

    public bool CanCarrierAccommodate(int carrierId, double weight)
    {
        var agent = GetCarrierById(carrierId);
        if (agent == null)
            return false;

        return agent.IsActive() && agent.HasCapacity((int)weight);
    }

    private void ValidateCarrierAgent(CarrierAgent agent)
    {
        // Basic validation
        if (agent.GetCarrierId() <= 0)
            throw new ArgumentException("Carrier ID must be positive");

        if (string.IsNullOrWhiteSpace(agent.GetName()))
            throw new ArgumentException("Carrier name cannot be empty");

        if (agent.GetCapacity() <= 0)
            throw new ArgumentException("Carrier capacity must be positive");
    }
}
