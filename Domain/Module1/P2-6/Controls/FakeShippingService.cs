using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ProRental.Domain.Services;

public class FakeShippingService : IShippingOptionService
{
    public string Name => "Fake Shipping";

    // ✅ SINGLE OPTION (used sometimes)
    public ShippingOption GetShippingOption(DeliveryDuration duration)
    {
        var opt = new ShippingOption();

        switch (duration)
        {
            case DeliveryDuration.NextDay:
                opt.Init(1, 10, "Next Day");
                break;

            case DeliveryDuration.ThreeDays:
                opt.Init(2, 5, "Three Days");
                break;

            case DeliveryDuration.OneWeek:
                opt.Init(3, 2, "One Week");
                break;

            default:
                opt.Init(0, 0, "Unknown");
                break;
        }

        return opt;
    }

    // ✅ LIST OF OPTIONS (used by cost calculation)
    public List<ShippingOption> GetShippingOptions(string orderId)
    {
        var opt1 = new ShippingOption();
        opt1.Init(1, 10, "Next Day");

        var opt2 = new ShippingOption();
        opt2.Init(2, 5, "Three Days");

        var opt3 = new ShippingOption();
        opt3.Init(3, 2, "One Week");

        return new List<ShippingOption> { opt1, opt2, opt3 };
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