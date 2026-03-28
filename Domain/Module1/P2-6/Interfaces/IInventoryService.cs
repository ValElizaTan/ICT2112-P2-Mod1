using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Interfaces.Domain;

/// <summary>
/// Facade interface for Module 2 Inventory.
/// Team 6 uses the read-only query methods below.
/// The ProcessLoan write method is used by Module 1.
/// </summary>
public interface IInventoryService
{
    // ── Write (used by Module 1 Order Processing) ────────────────────────
    bool ProcessLoan(int orderId, int customerId, DateTime startDate, DateTime dueDate,
                     Dictionary<int, int> productQuantities);

    // ── Read (used by Team 6 Catalogue) ─────────────────────────────────
    List<Product>?          GetAllProducts();
    Product?                GetProduct(int productId);
    List<Product>?          GetProductByCategory(int categoryId);
    int                     CheckProductQuantity(int productId, InventoryStatus status);
    ProductStatus           CheckProductStatus(int productId);
    List<Inventoryitem>?    GetInventoryItemByStatus(InventoryStatus status);
    List<Product>?          GetProductsByStatus(ProductStatus status);

    // ── Write (used by Module 1 Return/Refund Processing) ──────────
    bool TriggerReturnProcess(int orderId);
}