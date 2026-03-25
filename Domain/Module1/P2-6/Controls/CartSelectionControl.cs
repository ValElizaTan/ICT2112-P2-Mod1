using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CartSelectionControl
{
    private readonly ICartMapper _cartMapper;

    public CartSelectionControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    public void ToggleSelectItem(int cartId, int productId, bool isSelected)
    {
        var cart = _cartMapper.FindById(cartId) ?? throw new InvalidOperationException($"Cart {cartId} was not found.");
        var item = cart.GetItem(productId) ?? throw new InvalidOperationException("Cart item was not found.");

        item.SetSelected(isSelected);
        _cartMapper.Update(cart);
    }

    public void SelectAllObtainable(int cartId)
    {
        var cart = _cartMapper.FindById(cartId) ?? throw new InvalidOperationException($"Cart {cartId} was not found.");

        foreach (var item in cart.GetItems())
        {
            item.SetSelected(true);
        }

        _cartMapper.Update(cart);
    }

    public void ClearSelection(int cartId)
    {
        var cart = _cartMapper.FindById(cartId) ?? throw new InvalidOperationException($"Cart {cartId} was not found.");

        foreach (var item in cart.GetItems())
        {
            item.SetSelected(false);
        }

        _cartMapper.Update(cart);
    }

    public List<Cartitem> GetSelectedItems(int cartId)
    {
        var cart = _cartMapper.FindById(cartId) ?? throw new InvalidOperationException($"Cart {cartId} was not found.");
        return cart.GetItems().Where(x => x.IsSelected()).ToList();
    }
}