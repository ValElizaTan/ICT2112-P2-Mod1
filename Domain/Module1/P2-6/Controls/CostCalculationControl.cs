using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CostCalculationControl : ICostCalculation
{
    private readonly IShippingOptionService _shippingOptionService;

    public CostCalculationControl(IShippingOptionService shippingOptionService)
    {
        _shippingOptionService = shippingOptionService;
    }

    // ========================
    // 1. CalculateRentalCost
    // ========================
    public CostSummary CalculateRentalCost(
        List<SelectedItem> items, int rentalPeriod)
    {
        decimal rentalCost = 0;

        foreach (var item in items)
        {
            var price = item.GetUnitPrice();
            var quantity = item.GetQuantity();

            rentalCost += price * quantity * rentalPeriod;
        }

        decimal deposit = CalculateDepositAmount(rentalCost);

        return new CostSummary
        {
            RentalCost = rentalCost,
            DepositAmount = deposit
        };
    }

    // ============================
    // 2. CalculateFinalOrderCost
    // ============================
    public CostSummary CalculateFinalOrderCost(
        CostSummary summary,
        DeliveryDuration deliveryType)
    {
        var shippingOption = _shippingOptionService.GetShippingOption(deliveryType);

        if (shippingOption == null)
            throw new Exception("Invalid delivery type selected.");

        decimal shippingCost = shippingOption.GetCost();

        summary.DeliveryCost = shippingCost;

        summary.FinalOrderCost =
            summary.RentalCost +
            summary.DepositAmount +
            shippingCost;

        return summary;
    }

    // ===========================
    // 3. CalculateCartItemCosts
    // ===========================
public List<CartItemCost> CalculateCartItemCosts(List<CartItem> items)
{
    var result = new List<CartItemCost>();

    if (items == null || !items.Any())
        return result;

    foreach (var item in items)
    {
        var price = item.GetProduct()?.GetPrice() ?? 0m;
        var quantity = item.GetQuantity();

        result.Add(new CartItemCost(item, price * quantity));
    }

    return result;
}

    // ===========================
    // 4. CalculateDepositAmount
    // ===========================
    public decimal CalculateDepositAmount(decimal rentalCost)
    {
        return rentalCost * 0.2m;
    }
}