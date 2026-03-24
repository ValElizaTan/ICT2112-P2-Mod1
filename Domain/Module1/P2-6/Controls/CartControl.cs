using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CartControl : ICartService
{
    private readonly ICartMapper _cartMapper;

    public CartControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    // =========================
    // Cart retrieval
    // =========================

    public Cart GetOrCreateActiveCartBySession(string sessionId)
    {
        return _resolveActiveCart(sessionId, null);
    }

    public Cart GetOrCreateActiveCartByCustomer(string customerId)
    {
        return _resolveActiveCart(null, customerId);
    }

    public Cart MergeSessionCartToCustomerCart(string sessionId, string customerId)
    {
        var sessionCart  = _cartMapper.GetCartBySession(sessionId);
        var customerCart = _cartMapper.GetCartByCustomer(customerId);

        if (sessionCart == null) return customerCart;

        foreach (var item in sessionCart.GetItems())
        {
            customerCart.AddItem(item);
        }

        _cartMapper.Save(customerCart);
        return customerCart;
    }

    public Cart GetCart(string cartId)
    {
        return _cartMapper.GetCart(cartId);
    }

    // =========================
    // Cart operations
    // =========================

    public void AddToCart(string cartId, int productId, int qty)
    {
        var cart = GetCart(cartId);
        cart.AddItem(productId, qty);
        _cartMapper.Save(cart);
    }

    public void UpdateQuantity(string cartId, int productId, int qty)
    {
        var cart = GetCart(cartId);
        cart.UpdateQuantity(productId, qty);
        _cartMapper.Save(cart);
    }

    public void RemoveItem(string cartId, int productId)
    {
        var cart = GetCart(cartId);
        cart.RemoveItem(productId);
        _cartMapper.Save(cart);
    }

    public void EmptyCart(string cartId)
    {
        var cart = GetCart(cartId);
        cart.EmptyCart();
        _cartMapper.Save(cart);
    }

    // =========================
    // Rental period
    // =========================

    public void SetRentalPeriod(string cartId, DateTime startDate, DateTime endDate)
    {
        var cart = GetCart(cartId);
        cart.SetRentalPeriod(startDate, endDate);
        _cartMapper.Save(cart);
    }

    // =========================
    // Selection logic
    // =========================

    public void ToggleSelectItem(string cartId, int productId, bool isSelected)
    {
        var cart = GetCart(cartId);
        cart.ToggleSelect(productId, isSelected);
        _cartMapper.Save(cart);
    }

    public void SelectAllObtainable(string cartId)
    {
        var cart = GetCart(cartId);
        cart.SelectAll();
        _cartMapper.Save(cart);
    }

    public void ClearSelection(string cartId)
    {
        var cart = GetCart(cartId);
        cart.ClearSelection();
        _cartMapper.Save(cart);
    }

    public List<CartItem> GetSelectedItems(string cartId)
    {
        var cart = GetCart(cartId);
        return cart.GetSelectedItems();
    }

    // =========================
    // Display / Cost
    // =========================

    public List<CartItem> GetCartDisplaySummary(string cartId)
    {
        var cart = GetCart(cartId);
        return cart.GetItems();
    }

    public CostSummary GetCartCosts(string cartId)
    {
        var cart  = GetCart(cartId);
        decimal total = 0;

        foreach (var item in cart.GetSelectedItems())
        {
            total += item.GetQuantity() * item.GetProduct().GetPrice();
        }

        return new CostSummary
        {
            RentalCost    = total,
            DepositAmount = total * 0.2m
        };
    }

    public bool CanProceedToCheckout(string cartId)
    {
        var cart = GetCart(cartId);
        return cart.GetSelectedItems().Any();
    }

    public OrderDraft BuildSelectedOrderDraft(string cartId)
    {
        var cart = GetCart(cartId);

        return new OrderDraft
        {
            Items     = cart.GetSelectedItems(),
            StartDate = cart.GetStartDate(),
            EndDate   = cart.GetEndDate()
        };
    }

    // =========================
    // PRIVATE HELPERS
    // =========================

    private Cart _resolveActiveCart(string? sessionId, string? customerId)
    {
        Cart? cart = null;

        if (!string.IsNullOrEmpty(customerId))
            cart = _cartMapper.GetCartByCustomer(customerId);

        if (cart == null && !string.IsNullOrEmpty(sessionId))
            cart = _cartMapper.GetCartBySession(sessionId);

        if (cart == null)
        {
            cart = new Cart();
            _cartMapper.Save(cart);
        }

        return cart;
    }
}