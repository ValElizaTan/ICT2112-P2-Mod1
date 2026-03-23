using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CartQueryControl
{
    private readonly ICartMapper _cartMapper;

    public CartQueryControl(ICartMapper cartMapper)
    {
        _cartMapper = cartMapper;
    }

    public List<CartDisplayItem> GetCartDisplayItems(int cartId)
    {
        var cart = _cartMapper.FindById(cartId);

        if (cart == null)
        {
            return new List<CartDisplayItem>();
        }

        return cart.GetItems()
            .OrderBy(ci => ci.GetProductId())
            .Select(ci => new CartDisplayItem
            {
                ProductId = ci.GetProductId(),
                ProductName = "Product " + ci.GetProductId(),
                Quantity = ci.GetQuantity(),
                IsSelected = ci.IsSelected()
            })
            .ToList();
    }

    public CostSummary GetCartDisplaySummary(int cartId)
    {
        var cart = _cartMapper.FindById(cartId)
                   ?? throw new InvalidOperationException($"Cart {cartId} was not found.");

        return new CostSummary
        {
            RentalCost = 0m,
            DepositAmount = 0m,
            DeliveryCost = 0m,
            FinalOrderCost = 0m,
            UnobtainableItems = new List<UnobtainableItemInfo>()
        };
    }
}