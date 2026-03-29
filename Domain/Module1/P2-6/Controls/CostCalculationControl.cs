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
        List<SelectedItem> items,
        int rentalPeriod,
        int shippingOptionId)
    {
        var rentalSummary = CalculateRentalCost(items, rentalPeriod);

        decimal deliveryFee = 0;

        var options = _shippingOptionService.GetShippingOptions("");
        var selected = options.FirstOrDefault(
        o => o.GetOptionId() == shippingOptionId
    );

        if (selected != null)
        {
            deliveryFee = selected.GetCost();
        }

        return new CostSummary
        {
            RentalCost = rentalSummary.RentalCost,
            DepositAmount = rentalSummary.DepositAmount,
            DeliveryFee = deliveryFee,
            TotalCost = rentalSummary.RentalCost + rentalSummary.DepositAmount + deliveryFee
        };
    }
    // ===========================
    // 3. CalculateCartItemCosts
    // ===========================
    public List<CartItemCost> CalculateCartItemCosts(List<Cartitem> items)
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
            decimal deposit = rentalCost * 0.1m;
            return deposit > 10 ? deposit : 10m;
    }
}