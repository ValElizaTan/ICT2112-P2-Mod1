using ProRental.Interfaces.Domain;
using ProRental.Domain.Entities;

namespace ProRental.Data;

public class CartMapper : ICartMapper
{
    private static Cart _cart = CreateCart();

    private readonly IInventoryService _inventoryService;

    public CartMapper(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    private static Cart CreateCart()
    {
        var cart = new Cart();

        typeof(Cart)
            .GetField("_cartId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(cart, Guid.NewGuid().ToString());

        return cart;
    }

    public Cart? GetCartBySession(string sessionId)
    {
        PopulateProducts(_cart);
        return _cart;
    }

    public Cart? GetCartByCustomer(string customerId)
    {
        PopulateProducts(_cart);
        return _cart;
    }

    public Cart GetCart(string cartId)
    {
        PopulateProducts(_cart);
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

    // 🔥 THIS IS THE KEY FIX
    private void PopulateProducts(Cart cart)
    {
        var items = cart.GetItems();

        foreach (var item in items)
        {
            var productId = item.GetProductId();

            var product = _inventoryService.GetProduct(productId);

            if (product != null)
            {
                item.SetProduct(product);
            }
        }
    }
}