using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class ShippingOption
{
    public ShippingOption(decimal cost)
    {
        _cost = cost;
    }

    public decimal GetCost() => _cost ?? 0;

    public void SetCost(decimal cost) => _cost = cost;
}