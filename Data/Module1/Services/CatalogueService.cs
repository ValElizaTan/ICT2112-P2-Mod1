using ProRental.Domain.Entities;
using ProRental.Domain.Enums;          // InventoryStatus, ProductStatus live here
using ProRental.Interfaces.Domain;

namespace ProRental.Data.Module6.Services;

/// <summary>
/// Real implementation of ICatalogueService.
/// Calls Module 2's IInventoryService for products/availability.
///
/// Register in Program.cs (Team 6 section):
///   builder.Services.AddScoped&lt;ICatalogueService, CatalogueService&gt;();
/// </summary>
public class CatalogueService : ICatalogueService
{
    private readonly IInventoryService _inventoryService;

    // Uncomment when Module 3 provides their interface:
    // private readonly IProductCarbonService _carbonService;

    public CatalogueService(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    // ── ICatalogueService implementation ────────────────────────────────

    public List<Product> GetProducts(SearchFilter filter)
    {
        var products = _inventoryService.GetAllProducts() ?? new List<Product>();

        // Category filter
        if (filter.CategoryId.HasValue && filter.CategoryId > 0)
            products = products
                .Where(p => p.GetCategoryId() == filter.CategoryId.Value)
                .ToList();

        // Keyword filter
        if (!string.IsNullOrWhiteSpace(filter.Keywords))
        {
            var kw = filter.Keywords.Trim().ToLower();
            products = products
                .Where(p => p.GetProductName().ToLower().Contains(kw)
                         || (p.GetDescription() ?? "").ToLower().Contains(kw))
                .ToList();
        }

        // Price range filter
        if (filter.MinPrice.HasValue)
            products = products.Where(p => p.GetPrice() >= filter.MinPrice.Value).ToList();

        if (filter.MaxPrice.HasValue)
            products = products.Where(p => p.GetPrice() <= filter.MaxPrice.Value).ToList();

        // Sort
        products = filter.SortBy switch
        {
            "price_asc"  => products.OrderBy(p => p.GetPrice()).ToList(),
            "price_desc" => products.OrderByDescending(p => p.GetPrice()).ToList(),
            "name_desc"  => products.OrderByDescending(p => p.GetProductName()).ToList(),
            _            => products.OrderBy(p => p.GetProductName()).ToList()
        };

        return products;
    }

    public Product? GetProductById(int productId)
        => _inventoryService.GetProduct(productId);

    public AvailabilityStatus GetProductAvailability(int productId)
    {
        var count  = _inventoryService.CheckProductQuantity(productId, InventoryStatus.AVAILABLE);
        var status = _inventoryService.CheckProductStatus(productId);

        return status switch
        {
            ProductStatus.UNAVAILABLE => new AvailabilityStatus
            {
                IsAvailable    = false,
                AvailableCount = 0,
                Label          = "Unavailable",
                BadgeClass     = "bg-secondary"
            },
            ProductStatus.RETIRED => new AvailabilityStatus
            {
                IsAvailable    = false,
                AvailableCount = 0,
                Label          = "Retired",
                BadgeClass     = "bg-dark"
            },
            _ => count switch
            {
                0 => new AvailabilityStatus
                {
                    IsAvailable    = false,
                    AvailableCount = 0,
                    Label          = "Out of Stock",
                    BadgeClass     = "bg-danger"
                },
                <= 2 => new AvailabilityStatus
                {
                    IsAvailable    = true,
                    AvailableCount = count,
                    Label          = $"Low Stock ({count} left)",
                    BadgeClass     = "bg-warning text-dark"
                },
                _ => new AvailabilityStatus
                {
                    IsAvailable    = true,
                    AvailableCount = count,
                    Label          = "In Stock",
                    BadgeClass     = "bg-success"
                }
            }
        };
    }

    public CarbonFootprint? GetCarbonFootprint(int productId)
    {
        // TODO: swap for real Module 3 call once wired
        return new CarbonFootprint
        {
            ProductId  = productId,
            TotalCo2Kg = 0,
            BadgeLabel = "Pending",
            BadgeClass = "bg-secondary"
        };
    }

    public decimal GetProductPrice(int productId)
        => _inventoryService.GetProduct(productId)?.GetPrice() ?? 0m;
}