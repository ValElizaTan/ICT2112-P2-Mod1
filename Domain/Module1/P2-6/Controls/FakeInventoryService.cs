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
            CreateProduct(1, "Camera A", 50m, "DSLR Camera",    categoryId: 1, categoryName: "Cameras"),
            CreateProduct(2, "Lens B",   30m, "Zoom Lens",      categoryId: 2, categoryName: "Lenses"),
            CreateProduct(3, "Tripod C", 20m, "Stable tripod",  categoryId: 3, categoryName: "Accessories"),
        };
    }

    public Product? GetProduct(int productId)
        => GetAllProducts()?.FirstOrDefault(p => p.GetProductId() == productId);

    public List<Product>? GetProductByCategory(int categoryId)
        => GetAllProducts()?.Where(p => p.GetCategoryId() == categoryId).ToList();

    public int CheckProductQuantity(int productId, InventoryStatus status)
        => 10; // dummy stock

    public ProductStatus CheckProductStatus(int productId)
        => ProductStatus.AVAILABLE;

    public List<Inventoryitem>? GetInventoryItemByStatus(InventoryStatus status)
        => new List<Inventoryitem>();

    public List<Product>? GetProductsByStatus(ProductStatus status)
        => GetAllProducts();

    // ── Helper ────────────────────────────────────────

    private Product CreateProduct(int id, string name, decimal price, string desc,
                                  int categoryId = 0, string categoryName = "")
    {
        var product = new Product();
        var detail  = new Productdetail();

        // ── Populate Productdetail fields ──────────────────────────────────
        // These are direct private backing fields declared in the auto-generated
        // Productdetail class, so GetField() with NonPublic|Instance finds them.

        SetField(detail, typeof(Productdetail), "_name",        name);
        SetField(detail, typeof(Productdetail), "_price",       price);
        SetField(detail, typeof(Productdetail), "_description", desc);

        // ── Populate Product fields ────────────────────────────────────────

        // _productid is a plain private backing field — found directly.
        SetField(product, typeof(Product), "_productid", id);

        // _categoryid is a plain private backing field — found directly.
        SetField(product, typeof(Product), "_categoryid", categoryId);

        // FIX 1: "Productdetail" is declared as an AUTO-PROPERTY in the base
        //         Product entity:  public virtual Productdetail? Productdetail { get; private set; }
        //         The C# compiler generates the backing field name:
        //             <Productdetail>k__BackingField
        //         NOT "_productdetail".  Using the wrong name returns null from
        //         GetField(), the ?.SetValue() is silently skipped, and every
        //         product card shows "(unnamed)" / $0.00.
        SetField(product, typeof(Product), "<Productdetail>k__BackingField", detail);

        // FIX 2: Category is also an auto-property.  Populate it so that
        //         GetCategoryName() works in both the Fake and the real service.
        if (categoryId > 0 && !string.IsNullOrWhiteSpace(categoryName))
        {
            var cat = new Category();
            SetField(cat, typeof(Category), "_categoryid", categoryId);
            SetField(cat, typeof(Category), "_name",       categoryName);
            SetField(product, typeof(Product), "<Category>k__BackingField", cat);
        }

        // NOTE: Do NOT set Product._price (the extra field added by the P2-3
        //       partial class).  GetPrice() must read from Productdetail, not
        //       that unmapped field.  See the fix in Product.P2-3.cs below.

        return product;
    }

    // ── Reflection helper ─────────────────────────────
    private static void SetField(object target, Type type, string fieldName, object? value)
    {
        type.GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
            ?.SetValue(target, value);
    }
}