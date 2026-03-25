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
        var existing = _cartMapper.FindActiveBySessionId(sessionId);
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
        var sessionCart = _cartMapper.FindActiveBySessionId(sessionId);
        var customerCart = _cartMapper.FindActiveByCustomerId(customerId);

        // No ACTIVE session cart
        if (sessionCart == null)
        {
            if (customerCart != null)
            {
                return customerCart.GetCartId();
            }

            var newCustomerCart = new Cart();
            newCustomerCart.SetCustomerId(customerId);
            newCustomerCart.SetStatus(CartStatus.ACTIVE);

            _cartMapper.Insert(newCustomerCart);
            return newCustomerCart.GetCartId();
        }

        // ACTIVE session cart exists but no ACTIVE customer cart
        if (customerCart == null)
        {
            var newCustomerCart = new Cart();
            newCustomerCart.SetCustomerId(customerId);
            newCustomerCart.SetStatus(CartStatus.ACTIVE);

            _cartMapper.Insert(newCustomerCart);

            foreach (var item in sessionCart.GetItems())
            {
                var mergedItem = new Cartitem();
                mergedItem.SetCartId(newCustomerCart.GetCartId());
                mergedItem.SetProductId(item.GetProductId());
                mergedItem.SetQuantity(item.GetQuantity());
                mergedItem.SetSelected(item.IsSelected());

                newCustomerCart.AddItem(mergedItem);
            }

            _cartMapper.Update(newCustomerCart);

            sessionCart.SetStatus(CartStatus.EXPIRED);
            _cartMapper.Update(sessionCart);

            return newCustomerCart.GetCartId();
        }

        // Both ACTIVE carts exist -> merge session cart items into customer cart
        foreach (var item in sessionCart.GetItems())
        {
            var existingItem = customerCart.GetItem(item.GetProductId());

            if (existingItem == null)
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
                existingItem.SetQuantity(existingItem.GetQuantity() + item.GetQuantity());

                if (item.IsSelected())
                {
                    existingItem.SetSelected(true);
                }
            }
        }

        customerCart.SetStatus(CartStatus.ACTIVE);
        _cartMapper.Update(customerCart);

        sessionCart.SetStatus(CartStatus.EXPIRED);
        _cartMapper.Update(sessionCart);

        return customerCart.GetCartId();
    }
}