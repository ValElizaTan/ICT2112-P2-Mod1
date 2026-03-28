using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

/// <summary>
/// Catalogue service interface for Team 6 — Catalog Browsing.
/// Calls into Module 2's IInventoryService for product data.
/// </summary>
public interface ICatalogueService
{
    // ── Product Queries ──────────────────────────────────────────────────

    /// <summary>Returns all products available for browsing.</summary>
    List<Product> GetProducts(SearchFilter filter);

    /// <summary>Returns a single product by its ID.</summary>
    Product? GetProductById(int productId);

    /// <summary>Returns a human-readable availability label for a product.</summary>
    AvailabilityStatus GetProductAvailability(int productId);

    /// <summary>Returns the carbon footprint summary for a product (from Module 3).</summary>
    CarbonFootprint? GetCarbonFootprint(int productId);

    /// <summary>Returns the rental price for a product.</summary>
    decimal GetProductPrice(int productId);

    List<Category> GetCategories();
}