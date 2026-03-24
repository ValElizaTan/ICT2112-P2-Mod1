using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProRental.Domain.Services;

public class FakeShippingService : IShippingOptionService
{
    public List<ShippingOption> GetShippingOptions(string orderId)
    {
        return new List<ShippingOption>
        {
            new ShippingOption(5),
            new ShippingOption(10)
        };
    }

    public void ApplyCustomerSelection(string orderId, string optionId, string preference)
    {
        // no-op for now
    }

    public List<ShippingOption> BuildOptionSet(Order order)
    {
        return new List<ShippingOption>
        {
            new ShippingOption(5),
            new ShippingOption(10)
        };
    }

    public IActionResult SelectShippingOption(string orderId, string optionId)
    {
        return new OkResult();
    }

    public IActionResult CompareOptions(string orderId)
    {
        return new OkResult();
    }
}