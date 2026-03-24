using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CostCalculationControl : ICostCalculation
{
    private readonly IShippingOptionService _shippingOptionService;

    public CostCalculationControl(
        IShippingOptionService shippingOptionService)
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
            var product = item.GetProduct();
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
    public CostSummary CalculateFinalOrderCost(CostSummary summary, string orderId)
    {
        var shippingOptions = _shippingOptionService.GetShippingOptions(orderId);
        var shippingOption = shippingOptions.FirstOrDefault();

        decimal shippingCost = shippingOption?.GetCost() ?? 0;

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
    // public List<(CartItem Item, decimal Cost)> CalculateCartItemCosts
    // (List<CartItem> items)
    // {
    //     var result = new List<(CartItem, decimal)>();

    //     if (items == null || !items.Any())
    //         return result;

    //     foreach (var item in items)
    //     {
    //         var product = item.GetProduct();
    //         var price = product?.GetPrice() ?? 0;
    //         var quantity = item.GetQuantity();

    //         result.Add((item, price * quantity));
    //     }

    //     return result;
    // }

    // ===========================
    // 4. CalculateDepositAmount
    // ===========================
    public decimal CalculateDepositAmount(decimal rentalCost)
    {
        return rentalCost * 0.2m;
    }
}