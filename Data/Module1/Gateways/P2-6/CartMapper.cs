using ProRental.Interfaces.Domain;
using ProRental.Domain.Entities;

namespace ProRental.Data;

public class CartMapper : ICartMapper
{
    private static Cart _cart = CreateCart();

    private static Cart CreateCart()
    {
        var cart = new Cart();

        // set cartId via reflection (since no setter)
        typeof(Cart)
            .GetField("_cartId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(cart, Guid.NewGuid().ToString());

        return cart;
    }

    public Cart? GetCartBySession(string sessionId)
    {
        return _cart;
    }

    public Cart? GetCartByCustomer(string customerId)
    {
        return _cart;
    }

    public Cart GetCart(string cartId)
    {
        return _cart;
    }

    public void Save(Cart cart)
    {
        _cart = cart;
    }

    public void Delete(string cartId)
    {
        _cart = CreateCart();
    }
}