using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProRental.Domain.Services;

public class FakeShippingService : IShippingOptionService
{
    public string Name => "Fake Shipping";

    // ✅ MAIN METHOD USED BY COST CALCULATION
    public ShippingOption GetShippingOption(DeliveryDuration duration)
    {
        return duration switch
        {
            DeliveryDuration.NextDay   => new ShippingOption(10m),
            DeliveryDuration.ThreeDays => new ShippingOption(5m),
            DeliveryDuration.OneWeek   => new ShippingOption(2m),
            _ => new ShippingOption(0m)
        };
    }

    // =========================
    // Keep these for compatibility
    // =========================

    public List<ShippingOption> GetShippingOptions(string orderId)
    {
        return new List<ShippingOption>
        {
            new ShippingOption(10m),
            new ShippingOption(5m),
            new ShippingOption(2m)
        };
    }

    public void ApplyCustomerSelection(string orderId, string optionId, string preference)
    {
        // no-op
    }

    public List<ShippingOption> BuildOptionSet(Order order)
    {
        return GetShippingOptions(order?.OrderId.ToString() ?? "");
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
        return 0m;
    }

    public bool IsAvailable(int cartId)
    {
        return true;
    }
}