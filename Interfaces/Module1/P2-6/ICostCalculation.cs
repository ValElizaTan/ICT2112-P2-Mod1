using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface ICostCalculation
{
    CostSummary CalculateRentalCost(List<SelectedItem> items, int rentalPeriod);
    CostSummary CalculateFinalOrderCost(
    List<SelectedItem> items,
    int rentalPeriod,
    string shippingOptionId
);
    List<CartItemCost> CalculateCartItemCosts(List<CartItem> items);
    decimal CalculateDepositAmount(decimal rentalCost);
}