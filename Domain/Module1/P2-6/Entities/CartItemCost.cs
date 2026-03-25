namespace ProRental.Domain.Entities;

public class CartItemCost
{
    public Cartitem Item { get; set; }
    public decimal Cost { get; set; }

    public CartItemCost(Cartitem item, decimal cost)
    {
        Item = item;
        Cost = cost;
    }
}