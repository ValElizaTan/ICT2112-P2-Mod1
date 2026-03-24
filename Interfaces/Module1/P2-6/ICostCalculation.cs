using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public interface ICostCalculation
{
    CostSummary CalculateRentalCost(List<SelectedItem> items, int rentalPeriod);
    CostSummary CalculateFinalOrderCost(CostSummary summary, DeliveryDuration deliveryType);
    List<CartItemCost> CalculateCartItemCosts(List<CartItem> items);
    decimal CalculateDepositAmount(decimal rentalCost);
}