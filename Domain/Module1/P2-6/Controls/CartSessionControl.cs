using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CartSessionControl
{
    private readonly ICartMapper _cartMapper;

    public CartSessionControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    public int GetOrCreateActiveCartIdBySession(int sessionId)
    {
        var existing = _cartMapper.FindActiveBySessionId(sessionId.ToString());
        if (existing != null)
        {
            return existing.GetCartId();
        }

        var cart = new Cart();
        cart.SetSessionId(sessionId);
        cart.SetStatus(CartStatus.ACTIVE);

        _cartMapper.Insert(cart);
        return cart.GetCartId();
    }

    public int GetOrCreateActiveCartIdByCustomer(int customerId)
    {
        var existing = _cartMapper.FindActiveByCustomerId(customerId);
        if (existing != null)
        {
            return existing.GetCartId();
        }

        var cart = new Cart();
        cart.SetCustomerId(customerId);
        cart.SetStatus(CartStatus.ACTIVE);

        _cartMapper.Insert(cart);
        return cart.GetCartId();
    }

    public int MergeSessionCartToCustomerCart(int sessionId, int customerId)
    {
        var sessionCart = _cartMapper.FindActiveBySessionId(sessionId.ToString());
        var customerCart = _cartMapper.FindActiveByCustomerId(customerId);

        if (sessionCart == null && customerCart == null)
        {
            var newCart = new Cart();
            newCart.SetCustomerId(customerId);
            newCart.SetStatus(CartStatus.ACTIVE);

            _cartMapper.Insert(newCart);
            return newCart.GetCartId();
        }

        if (sessionCart == null && customerCart != null)
        {
            return customerCart.GetCartId();
        }

        if (sessionCart != null && customerCart == null)
        {
            sessionCart.AssignToCustomer(customerId);
            sessionCart.SetStatus(CartStatus.ACTIVE);
            _cartMapper.Update(sessionCart);
            return sessionCart.GetCartId();
        }

        if (sessionCart == null || customerCart == null)
        {
            throw new InvalidOperationException("Unable to merge carts.");
        }

        foreach (var item in sessionCart.GetItems())
        {
            var existing = customerCart.GetItem(item.GetProductId());
            if (existing == null)
            {
                var mergedItem = new Cartitem();
                mergedItem.SetCartId(customerCart.GetCartId());
                mergedItem.SetProductId(item.GetProductId());
                mergedItem.SetQuantity(item.GetQuantity());
                mergedItem.SetSelected(item.IsSelected());

                customerCart.AddItem(mergedItem);
            }
            else
            {
                existing.SetQuantity(existing.GetQuantity() + item.GetQuantity());
            }
        }

        customerCart.SetStatus(CartStatus.ACTIVE);

        _cartMapper.Update(customerCart);
        _cartMapper.Delete(sessionCart.GetCartId());

        return customerCart.GetCartId();
    }
}