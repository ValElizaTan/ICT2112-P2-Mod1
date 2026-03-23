namespace ProRental.Domain.Entities;

public class CartDisplayItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsSelected { get; set; }
    public bool IsObtainable { get; set; }
    public int? AvailableQuantity { get; set; }
    public int RentalDays { get; set; }
    public decimal CartItemPrice { get; set; }
    public decimal Subtotal { get; set; }
}