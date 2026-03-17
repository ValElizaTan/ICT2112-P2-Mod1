namespace ProRental.Domain.Entities;

public partial class CartItem
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public bool IsSelected { get; set; } = true;
}