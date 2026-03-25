namespace ProRental.Domain.Entities;

/// <summary>
/// Lightweight carbon footprint summary consumed by the catalogue.
/// Populated via Module 3's IProductCarbonService.
/// </summary>
public class CarbonFootprint
{
    public int     ProductId    { get; set; }
    public double  TotalCo2Kg  { get; set; }
    public string  BadgeLabel   { get; set; } = string.Empty;  // e.g. "Eco Friendly", "Moderate", "High Impact"
    public string  BadgeClass   { get; set; } = string.Empty;  // Bootstrap badge colour
}