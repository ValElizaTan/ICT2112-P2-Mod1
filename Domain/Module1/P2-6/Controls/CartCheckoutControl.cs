using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CartCheckoutControl
{
    private readonly ICartMapper _cartMapper;

    public CartCheckoutControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    public bool CanProceedToCheckout(int cartId)
    {
        var cart = _cartMapper.FindById(cartId);
        if (cart is null || cart.IsEmpty())
        {
            return false;
        }

        if (cart.GetRentalStart() == DateTime.MinValue || cart.GetRentalEnd() == DateTime.MinValue)
        {
            return false;
        }

        if (cart.GetRentalEnd() <= cart.GetRentalStart())
        {
            return false;
        }

        return cart.GetItems().Any(x => x.IsSelected());
    }
}