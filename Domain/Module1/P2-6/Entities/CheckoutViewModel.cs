using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Module6.Entities;

public class CheckoutViewModel
{
    public OrderDraft    Draft       { get; set; } = new();
    public CostSummary   Costs       { get; set; } = new();
    public DateTime      StartDate   { get; set; } = DateTime.Today.AddDays(1);
    public DateTime      EndDate     { get; set; } = DateTime.Today.AddDays(2);
    public DeliveryDuration DeliveryType { get; set; } = DeliveryDuration.NextDay;
    public bool          NotifyOptIn { get; set; }
}