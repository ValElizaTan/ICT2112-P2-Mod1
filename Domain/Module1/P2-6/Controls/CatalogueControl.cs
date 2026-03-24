using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module6.Controls;

/// <summary>
/// Domain Control for Catalogue Browsing (Team 6).
/// Orchestrates search, filtering, availability checks, and add-to-order.
/// Depends on ICatalogueService (Module 2 facade) for all data access.
/// </summary>
public class CatalogueControl
{
    private readonly ICatalogueService _catalogueService;

    public CatalogueControl(ICatalogueService catalogueService)
    {
        _catalogueService = catalogueService;
    }

    // ── Search / Browse ──────────────────────────────────────────────────

    /// <summary>Searches products using the supplied filter and returns a paged result.</summary>
    public PagedResult<Product> SearchProducts(SearchFilter filter)
    {
        var all     = _catalogueService.GetProducts(filter);
        var total   = all.Count;
        var items   = all
            .Skip((filter.CurrentPage - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        return new PagedResult<Product>
        {
            Items       = items,
            TotalCount  = total,
            CurrentPage = filter.CurrentPage,
            PageSize    = filter.PageSize
        };
    }

    /// <summary>Returns a single product, or null if not found.</summary>
    public Product? GetProductById(int productId)
    {
        ValidateProductId(productId);
        return _catalogueService.GetProductById(productId);
    }

    /// <summary>Returns the availability status for display on a product card/detail.</summary>
    public AvailabilityStatus GetAvailability(int productId)
    {
        ValidateProductId(productId);
        return _catalogueService.GetProductAvailability(productId);
    }

    /// <summary>Returns the carbon footprint badge for a product (Module 3 data).</summary>
    public CarbonFootprint? GetCarbonFootprint(int productId)
    {
        ValidateProductId(productId);
        return _catalogueService.GetCarbonFootprint(productId);
    }

    // ── Add to Order ─────────────────────────────────────────────────────

    /// <summary>
    /// Creates a SelectedItem after validating the product and quantity.
    /// Throws if the product is unavailable or quantity is invalid.
    /// </summary>
    public SelectedItem AddProductSelection(Product product, int quantity)
    {
        ValidateProduct(product.GetProductId());
        ValidateQuantity(quantity);
        CheckAvailabilityBeforeSelection(product.GetProductId(), quantity);
        return new SelectedItem(product, quantity);
    }

    // ── Private Guards ────────────────────────────────────────────────────

    private void ValidateProductId(int productId)
    {
        if (productId <= 0)
            throw new ArgumentException("Product ID must be a positive integer.", nameof(productId));
    }

    private void ValidateProduct(int productId)
    {
        var product = _catalogueService.GetProductById(productId);
        if (product == null)
            throw new InvalidOperationException($"Product {productId} does not exist.");
    }

    private void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be at least 1.", nameof(quantity));
    }

    private void CheckAvailabilityBeforeSelection(int productId, int quantity)
    {
        var avail = _catalogueService.GetProductAvailability(productId);
        if (!avail.IsAvailable)
            throw new InvalidOperationException($"Product {productId} is currently unavailable.");
        if (avail.AvailableCount < quantity)
            throw new InvalidOperationException(
                $"Only {avail.AvailableCount} unit(s) available; requested {quantity}.");
    }
}