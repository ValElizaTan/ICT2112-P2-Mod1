using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CartControl : ICartService
{
    private readonly CartSessionControl _cartSessionCtrl;
    private readonly CartItemControl _cartItemCtrl;
    private readonly CartSelectionControl _cartSelectionCtrl;
    private readonly CartQueryControl _cartQueryCtrl;
    private readonly CartCheckoutControl _cartCheckoutCtrl;

    public CartControl(
        CartSessionControl cartSessionCtrl,
        CartItemControl cartItemCtrl,
        CartSelectionControl cartSelectionCtrl,
        CartQueryControl cartQueryCtrl,
        CartCheckoutControl cartCheckoutCtrl)
    {
        _cartSessionCtrl = cartSessionCtrl;
        _cartItemCtrl = cartItemCtrl;
        _cartSelectionCtrl = cartSelectionCtrl;
        _cartQueryCtrl = cartQueryCtrl;
        _cartCheckoutCtrl = cartCheckoutCtrl;
    }

    public int GetOrCreateActiveCartIdBySessionId(int sessionId) => _cartSessionCtrl.GetOrCreateActiveCartIdBySession(sessionId);

    public int GetOrCreateActiveCartIdByCustomerId(int customerId) => _cartSessionCtrl.GetOrCreateActiveCartIdByCustomer(customerId);

    public int MergeSessionCartToCustomerCart(int sessionId, int customerId) => _cartSessionCtrl.MergeSessionCartToCustomerCart(sessionId, customerId);

    public Cart GetCart(int cartId) => _cartItemCtrl.GetCart(cartId);
    
    public void AddToCart(int cartId, int productId, int qty) => _cartItemCtrl.AddToCart(cartId, productId, qty);

    public void UpdateQuantity(int cartId, int productId, int qty) => _cartItemCtrl.UpdateQuantity(cartId, productId, qty);

    public void RemoveItem(int cartId, int productId) => _cartItemCtrl.RemoveItem(cartId, productId);

    public void EmptyCart(int cartId) => _cartItemCtrl.EmptyCart(cartId);

    public List<string> SetRentalPeriod(int cartId, DateTime start, DateTime end) => _cartItemCtrl.SetRentalPeriod(cartId, start, end);

    public void ToggleSelectItem(int cartId, int productId, bool isSelected) => _cartSelectionCtrl.ToggleSelectItem(cartId, productId, isSelected);

    public void SelectAllObtainable(int cartId) => _cartSelectionCtrl.SelectAllObtainable(cartId);

    public void ClearSelection(int cartId) => _cartSelectionCtrl.ClearSelection(cartId);

    public List<Cartitem> GetSelectedItems(int cartId) => _cartSelectionCtrl.GetSelectedItems(cartId);

    public CostSummary GetCartDisplaySummary(int cartId) => _cartQueryCtrl.GetCartDisplaySummary(cartId);

    public List<CartDisplayItem> GetCartDisplayItems(int cartId) => _cartQueryCtrl.GetCartDisplayItems(cartId);

    public bool CanProceedToCheckout(int cartId) => _cartCheckoutCtrl.CanProceedToCheckout(cartId);
}