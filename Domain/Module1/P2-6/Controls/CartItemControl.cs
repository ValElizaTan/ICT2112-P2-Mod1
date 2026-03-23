using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CartItemControl
{
    private readonly ICartMapper _cartMapper;

    public CartItemControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    public Cart GetCart(int cartId)
        => _cartMapper.FindById(cartId) ?? throw new InvalidOperationException($"Cart {cartId} was not found.");

    public void AddToCart(int cartId, int productId, int qty)
    {
        if (qty <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(qty), "Quantity must be greater than zero.");
        }

        var cart = GetCart(cartId);
        var existing = cart.GetItem(productId);

        if (existing is null)
        {
            var item = new Cartitem();
            item.SetCartId(cart.GetCartId());
            item.SetProductId(productId);
            item.SetQuantity(qty);
            item.SetSelected(true);

            cart.AddItem(item);
        }
        else
        {
            existing.SetQuantity(existing.GetQuantity() + qty);
        }

        _cartMapper.Update(cart);
    }

    public void UpdateQuantity(int cartId, int productId, int qty)
    {
        var cart = GetCart(cartId);
        var item = cart.GetItem(productId) ?? throw new InvalidOperationException("Cart item was not found.");

        if (qty == 0)
        {
            cart.RemoveItem(productId);
        }
        else
        {
            item.SetQuantity(qty);
        }

        _cartMapper.Update(cart);
    }

    public void RemoveItem(int cartId, int productId)
    {
        var cart = GetCart(cartId);
        cart.RemoveItem(productId);
        _cartMapper.Update(cart);
    }

    public void EmptyCart(int cartId)
    {
        var cart = GetCart(cartId);
        cart.EmptyCart();
        _cartMapper.Update(cart);
    }

    public List<string> SetRentalPeriod(int cartId, DateTime start, DateTime end)
    {
        var warnings = new List<string>();

        var cart = _cartMapper.FindById(cartId);
        if (cart == null)
        {
            warnings.Add("Cart not found.");
            return warnings;
        }

        var startUtc = DateTime.SpecifyKind(start.Date, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end.Date, DateTimeKind.Utc);

        cart.SetRentalPeriod(startUtc, endUtc);

        _cartMapper.Update(cart);
        return warnings;
    }
}