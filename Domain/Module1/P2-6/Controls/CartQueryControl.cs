using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CartQueryControl
{
    private readonly ICartMapper _cartMapper;
    private readonly ICostCalculation _costCalculation;

    public CartQueryControl(ICartMapper cartMapper, ICostCalculation costCalculation)
    {
        _cartMapper = cartMapper;
        _costCalculation = costCalculation;
    }

    public CostSummary GetCartDisplaySummary(int cartId)
    {
        var cart = _cartMapper.FindById(cartId);

        if (cart == null)
        {
            return new CostSummary
            {
                RentalCost = 0m,
                DepositAmount = 0m,
                TotalCost = 0m
            };
        }

        var selectedItems = cart.GetItems()
            .Where(x => x.IsSelected() && x.GetProduct() != null)
            .Select(ci => new SelectedItem(ci.GetProduct()!, ci.GetQuantity()))
            .ToList();

        int rentalDays = GetRentalDays(cart);

        if (!selectedItems.Any() || rentalDays <= 0)
        {
            return new CostSummary
            {
                RentalCost = 0m,
                DepositAmount = 0m,
                TotalCost = 0m
            };
        }

        var summary = _costCalculation.CalculateRentalCost(selectedItems, rentalDays);

        return new CostSummary
        {
            RentalCost = summary.RentalCost,
            DepositAmount = summary.DepositAmount,
            TotalCost = summary.RentalCost
        };
    }

    public List<CartDisplayItem> GetCartDisplayItems(int cartId)
    {
        var cart = _cartMapper.FindById(cartId);

        if (cart == null)
        {
            return new List<CartDisplayItem>();
        }

        int rentalDays = GetRentalDays(cart);
        var itemCosts = _costCalculation.CalculateCartItemCosts(cart.GetItems());

        return cart.GetItems()
            .Where(ci => ci.GetProduct() != null)
            .OrderBy(ci => ci.GetProductId())
            .Select(ci =>
            {
                var product = ci.GetProduct()!;
                var matchedCost = itemCosts.FirstOrDefault(x => x.Item.GetProductId() == ci.GetProductId());

                decimal itemPrice = 0m;

                if (product.Productdetail != null)
                {
                    itemPrice = product.Productdetail.GetPrice();
                }
                else
                {
                    itemPrice = product.GetPrice();
                }

                decimal subtotal = matchedCost?.Cost ?? 0m;

                return new CartDisplayItem
                {
                    ProductId = ci.GetProductId(),
                    ProductName = product.GetProductName(),
                    Quantity = ci.GetQuantity(),
                    IsSelected = ci.IsSelected(),
                    IsObtainable = true,
                    AvailableQuantity = null,
                    RentalDays = rentalDays,
                    CartItemPrice = itemPrice,
                    Subtotal = subtotal
                };
            })
            .ToList();
    }

    private int GetRentalDays(Cart cart)
    {
        var start = cart.GetRentalStart();
        var end = cart.GetRentalEnd();

        if (start == DateTime.MinValue || end == DateTime.MinValue)
        {
            return 0;
        }

        var days = (end.Date - start.Date).Days;
        return days > 0 ? days : 0;
    }
}