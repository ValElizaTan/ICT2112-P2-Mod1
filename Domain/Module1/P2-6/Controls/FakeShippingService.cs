using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProRental.Domain.Services;

public class FakeShippingService : IShippingOptionService
{
    public string Name => "Fake Shipping";

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

    public decimal CalculateCost(decimal subtotal, int rentalDays)
    {
        return 0m; // no-op for now
    }

    public bool IsAvailable(int cartId)
    {
        return true; // no-op for now
    }
}