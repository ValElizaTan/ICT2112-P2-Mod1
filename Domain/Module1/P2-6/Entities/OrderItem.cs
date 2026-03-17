namespace ProRental.Domain.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime? RentalStartDate { get; set; }
    public DateTime? RentalEndDate { get; set; }
}