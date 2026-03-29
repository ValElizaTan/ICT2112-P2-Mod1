using ProRental.Domain.Entities;

namespace ProRental.Domain.Module6.Entities;

/// <summary>
/// View model passed from CatalogueController to CatalogBrowsing.cshtml.
/// </summary>
public class CatalogueViewModel
{
    // ── Products ──────────────────────────────────────────────────────────
    public PagedResult<Product>                     PagedProducts  { get; set; } = new();
    public Dictionary<int, AvailabilityStatus>      Availability   { get; set; } = new();
    public Dictionary<int, CarbonFootprint?>        CarbonData     { get; set; } = new();

    // ── Filter state (echo back to view) ─────────────────────────────────
    public SearchFilter                             Filter         { get; set; } = new();

    // ── For dropdowns ─────────────────────────────────────────────────────
    public List<Category>                           Categories     { get; set; } = new();

    // ── Feedback ──────────────────────────────────────────────────────────
    public string?                                  Message        { get; set; }
    public bool                                     IsError        { get; set; }
}