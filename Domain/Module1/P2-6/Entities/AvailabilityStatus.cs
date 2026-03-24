namespace ProRental.Domain.Entities;

/// <summary>
/// Availability status returned by the catalogue for a given product.
/// Maps to Module 2's ProductStatus / InventoryStatus.
/// </summary>
public class AvailabilityStatus
{
    public bool   IsAvailable    { get; set; }
    public int    AvailableCount { get; set; }
    public string Label          { get; set; } = string.Empty;   // e.g. "In Stock", "Low Stock", "Unavailable"
    public string BadgeClass     { get; set; } = string.Empty;   // Bootstrap badge class
}