using ProRental.Domain.Entities;

namespace ProRental.Domain.Module6.Entities;

/// <summary>
/// View model for a single product's detail view.
/// </summary>
public class ProductDetailViewModel
{
    public Product?            Product      { get; set; }
    public AvailabilityStatus? Availability { get; set; }
    public CarbonFootprint?    Carbon       { get; set; }
    public string?             Message      { get; set; }
}