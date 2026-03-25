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
}