using ProRental.Domain.Entities;
public class CartItemCost
{
    public CartItem Item { get; set; }
    public decimal Cost { get; set; }

    public CartItemCost(CartItem item, decimal cost)
    {
        Item = item;
        Cost = cost;
    }
}