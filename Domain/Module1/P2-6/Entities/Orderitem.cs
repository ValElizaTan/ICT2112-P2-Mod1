namespace ProRental.Domain.Entities;

public partial class Orderitem
{
    // ── Public accessors ─────────────────────────────────────────────────
    public int OrderItemId          => Orderitemid;
    public int OrderId              => Orderid;
    public int ProductId            => Productid;
    public int OrderedQuantity      => _quantity;    // Quantity is taken by scaffolded private
    public decimal UnitPrice        => Unitprice;
    public DateTime? RentalStart    => Rentalstartdate;
    public DateTime? RentalEnd      => Rentalenddate;

    // ── Factory method ───────────────────────────────────────────────────
    public static Orderitem Create(int productId, int quantity, decimal unitPrice,
                                   DateTime? rentalStart, DateTime? rentalEnd)
    {
        var item = new Orderitem();
        typeof(Orderitem).GetProperty("Productid")!.SetValue(item, productId);
        typeof(Orderitem).GetProperty("Quantity")!.SetValue(item, quantity);
        typeof(Orderitem).GetProperty("Unitprice")!.SetValue(item, unitPrice);
        typeof(Orderitem).GetProperty("Rentalstartdate")!.SetValue(item, rentalStart);
        typeof(Orderitem).GetProperty("Rentalenddate")!.SetValue(item, rentalEnd);
        return item;
    }
}