using System;
using System.Collections.Generic;
using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICartService
{
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
