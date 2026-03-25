namespace ProRental.Domain.Entities;

public class OrderDraft
{
    public List<Cartitem> Items      { get; set; } = new();
    public DateTime?      StartDate  { get; set; }
    public DateTime?      EndDate    { get; set; }
}