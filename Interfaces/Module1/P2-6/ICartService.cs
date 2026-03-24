using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICartService
{
    // ── Cart retrieval ───────────────────────────────────────────────────────
    Cart GetOrCreateActiveCartBySession(string sessionId);
    Cart GetOrCreateActiveCartByCustomer(string customerId);
    Cart MergeSessionCartToCustomerCart(string sessionId, string customerId);
    Cart GetCart(string cartId);

    // ── Cart operations ──────────────────────────────────────────────────────
    void AddToCart(string cartId, int productId, int qty);
    void UpdateQuantity(string cartId, int productId, int qty);
    void RemoveItem(string cartId, int productId);
    void EmptyCart(string cartId);

    // ── Rental period ────────────────────────────────────────────────────────
    void SetRentalPeriod(string cartId, DateTime startDate, DateTime endDate);

    // ── Selection logic ──────────────────────────────────────────────────────
    void ToggleSelectItem(string cartId, int productId, bool isSelected);
    void SelectAllObtainable(string cartId);
    void ClearSelection(string cartId);
    List<CartItem> GetSelectedItems(string cartId);

    // ── Display / Cost ───────────────────────────────────────────────────────
    List<CartItem>  GetCartDisplaySummary(string cartId);
    CostSummary     GetCartCosts(string cartId);
    bool            CanProceedToCheckout(string cartId);
    OrderDraft      BuildSelectedOrderDraft(string cartId);
}