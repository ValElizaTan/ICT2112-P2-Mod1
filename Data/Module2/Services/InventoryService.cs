using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Data.Services;

/// <summary>
/// Real database-backed implementation of IInventoryService.
/// Replaces FakeInventoryService so the Catalogue page reads from PostgreSQL.
///
/// FILE LOCATION:  Data/Module2/Services/InventoryService.cs
///
/// IMPORTANT — EF Core 9 LINQ rules applied throughout:
///   • Method calls like p.GetProductId() cannot be translated to SQL and will
///     throw InvalidOperationException at runtime inside an IQueryable chain.
///   • All server-side WHERE / FirstOrDefault filters use EF.Property<T>()
///     which maps directly to the private backing fields declared in AppDbContext.
///   • .Include() is mandatory — without it, navigation properties (Productdetail,
///     Category) are null after loading, causing "(unnamed)" and $0.00.
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly AppDbContext _db;

    public InventoryService(AppDbContext db)
    {
        _db = db;
    }

    // ── Read ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns ALL products with their Productdetail and Category eagerly loaded.
    /// Without .Include(), navigation properties stay null and GetProductName() /
    /// GetPrice() return "(unnamed)" and 0.00 respectively.
    /// </summary>
    public List<Product>? GetAllProducts()
    {
        return _db.Products
            .Include(p => p.Productdetail)
            .Include(p => p.Category)
            .ToList();
    }

    /// <summary>
    /// Returns a single product (with Productdetail + Category) by its primary key.
    ///
    /// FIX: Cannot use p.GetProductId() in a LINQ WHERE — EF Core 9 cannot
    /// translate arbitrary method calls to SQL and throws at runtime.
    /// Use EF.Property<int>(p, "Productid") instead, which maps to the
    /// private "Productid" property backed by _productid (configured in AppDbContext.cs).
    /// </summary>
    public Product? GetProduct(int productId)
    {
        return _db.Products
            .Include(p => p.Productdetail)
            .Include(p => p.Category)
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == productId);
    }

    /// <summary>
    /// Returns all products in a given category.
    ///
    /// FIX: Cannot use p.GetCategoryId() in LINQ — use EF.Property instead,
    /// mapping to the private "Categoryid" property backed by _categoryid.
    /// </summary>
    public List<Product>? GetProductByCategory(int categoryId)
    {
        return _db.Products
            .Include(p => p.Productdetail)
            .Include(p => p.Category)
            .Where(p => EF.Property<int>(p, "Categoryid") == categoryId)
            .ToList();
    }

    /// <summary>
    /// Counts inventory items for a product filtered by InventoryStatus.
    ///
    /// Both "Productid" and "Status" are private properties on Inventoryitem with
    /// no public accessors — EF.Property is the only safe way to filter on them
    /// inside an IQueryable chain.
    ///   "Productid" backed by _productid, mapped in AppDbContext.cs
    ///   "Status"    backed by _status,    mapped in AppDbContext.Custom.cs
    /// </summary>
    public int CheckProductQuantity(int productId, InventoryStatus status)
    {
        return _db.Inventoryitems
            .Where(i => EF.Property<int>(i, "Productid") == productId
                     && EF.Property<InventoryStatus>(i, "Status") == status)
            .Count();
    }

    /// <summary>
    /// Returns the ProductStatus for a given product ID.
    ///
    /// Loads the entity first (safe, tracked), then reads Status via the
    /// public GetProductStatus() accessor defined in the P2-2 Product partial.
    /// This avoids an untranslatable method call inside a LINQ query.
    /// </summary>
    public ProductStatus CheckProductStatus(int productId)
    {
        var product = _db.Products
            .FirstOrDefault(p => EF.Property<int>(p, "Productid") == productId);

        return product?.GetProductStatus() ?? ProductStatus.UNAVAILABLE;
    }

    /// <summary>
    /// Returns all inventory items that match the given status.
    /// "Status" is a private property mapped in AppDbContext.Custom.cs via HasField("_status").
    /// </summary>
    public List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status)
    {
        return _db.Inventoryitems
            .Where(i => EF.Property<InventoryStatus>(i, "Status") == status)
            .ToList();
    }

    /// <summary>
    /// Returns all products matching the given ProductStatus, with details loaded.
    /// "Status" is a private property mapped in AppDbContext.Custom.cs via HasField("_status").
    /// </summary>
    public List<Product>? GetProductsByStatus(ProductStatus status)
    {
        return _db.Products
            .Include(p => p.Productdetail)
            .Include(p => p.Category)
            .Where(p => EF.Property<ProductStatus>(p, "Status") == status)
            .ToList();
    }

    // ── Write ─────────────────────────────────────────────────────────────

    public bool ProcessLoan(int orderId, int customerId, DateTime startDate,
                            DateTime dueDate, Dictionary<int, int> productQuantities)
    {
        // Implement when Module 1 order processing wires this up
        return true;
    }
}