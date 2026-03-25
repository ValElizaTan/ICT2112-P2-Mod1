using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

public class FakeInventoryService : IInventoryService
{
    // ── Write ─────────────────────────────────────────
    public bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate,
                            Dictionary<int, int> productQuantities)
        => true;

    // ── Read ──────────────────────────────────────────

    public List<Product>? GetAllProducts()
    {
        return new List<Product>
        {
            CreateProduct(1, "Camera A", 50, "DSLR Camera"),
            CreateProduct(2, "Lens B", 30, "Zoom Lens"),
            CreateProduct(3, "Tripod C", 20, "Stable tripod")
        };
    }

    public Product? GetProduct(int productId)
    {
        return GetAllProducts()?.FirstOrDefault(p => p.GetProductId() == productId);
    }

    public List<Product>? GetProductByCategory(int categoryId)
    {
        return GetAllProducts();
    }

    public int CheckProductQuantity(int productId, InventoryStatus status)
    {
        return 10; // dummy stock
    }

    public ProductStatus CheckProductStatus(int productId)
    {
        return ProductStatus.AVAILABLE;
    }

    public List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status)
    {
        return new List<Inventoryitem>();
    }

    public List<Product>? GetProductsByStatus(ProductStatus status)
    {
        return GetAllProducts();
    }

    // ── Helper ────────────────────────────────────────
    private Product CreateProduct(int id, string name, decimal price, string desc)
    {
        var product = new Product();

        // hack: simulate productdetail
        var detail = new Productdetail();

        typeof(Productdetail).GetField("_name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(detail, name);

        typeof(Productdetail).GetField("_price", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(detail, price);

        typeof(Productdetail).GetField("_description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(detail, desc);

        typeof(Product).GetField("_productid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(product, id);

        typeof(Product).GetField("_productdetail", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(product, detail);

        return product;
    }
}