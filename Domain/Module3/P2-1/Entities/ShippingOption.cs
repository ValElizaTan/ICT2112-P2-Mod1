namespace ProRental.Domain.Entities;

public partial class ShippingOption
{
    public int GetOptionId()
    {
        return _optionId;
    }

    public decimal GetCost()
    {
        return _cost ?? 0;
    }

    public void SetCost(decimal cost)
    {
        _cost = cost;
    }

    public string GetDisplayName()
    {
        return _displayName ?? "";
    }

    public int GetDeliveryDays()
    {
        return _deliveryDays ?? 0;
    }

    // OPTIONAL helper for fake data
    public void Init(int id, decimal cost, string name)
    {
        _optionId = id;
        _cost = cost;
        _displayName = name;
    }
}