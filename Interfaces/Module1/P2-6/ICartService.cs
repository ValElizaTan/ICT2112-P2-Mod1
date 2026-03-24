<<<<<<< HEAD
using System;
using System.Collections.Generic;
=======
>>>>>>> origin/Catalauge,-Cart,-Checkout,-Payment-(Backup)
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICartService
{
<<<<<<< HEAD
    int GetOrCreateActiveCartIdBySession(int sessionId);
    int GetOrCreateActiveCartIdByCustomer(int customerId);
    int MergeSessionCartToCustomerCart(int sessionId, int customerId);

    Cart GetCart(int cartId);

    void AddToCart(int cartId, int productId, int qty);
    void UpdateQuantity(int cartId, int productId, int qty);
    void RemoveItem(int cartId, int productId);
    void EmptyCart(int cartId);
    List<string> SetRentalPeriod(int cartId, DateTime start, DateTime end);
    void ToggleSelectItem(int cartId, int productId, bool isSelected);
    void SelectAllObtainable(int cartId);
    void ClearSelection(int cartId);
    List<Cartitem> GetSelectedItems(int cartId);
    CostSummary GetCartDisplaySummary(int cartId);
    List<CartDisplayItem> GetCartDisplayItems(int cartId);
    bool CanProceedToCheckout(int cartId);
}
=======
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
>>>>>>> origin/Catalauge,-Cart,-Checkout,-Payment-(Backup)
