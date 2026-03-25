using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CartQueryControl
{
    private readonly ICartMapper _cartMapper;
    private readonly ICostCalculation _costCalculation;
    private readonly ICatalogueService _catalogueService;

    public CartQueryControl(
        ICartMapper cartMapper,
        ICostCalculation costCalculation,
        ICatalogueService catalogueService)
    {
        _cartMapper = cartMapper;
        _costCalculation = costCalculation;
        _catalogueService = catalogueService;
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

        var cartItems = cart.GetItems()
            .Where(x => x.IsSelected())
            .ToList();

        if (!cartItems.Any())
        {
            return new CostSummary
            {
                RentalCost = 0m,
                DepositAmount = 0m,
                TotalCost = 0m
            };
        }

        foreach (var item in cartItems)
        {
            var product = item.GetProduct() ?? _catalogueService.GetProductById(item.GetProductId());
            if (product != null)
            {
                item.SetProduct(product);
            }
        }

        var itemCosts = _costCalculation.CalculateCartItemCosts(cartItems);
        decimal totalItemCost = itemCosts.Sum(x => x.Cost);

        return new CostSummary
        {
            RentalCost = 0m,
            DepositAmount = 0m,
            TotalCost = totalItemCost
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

        return cart.GetItems()
            .OrderBy(ci => ci.GetProductId())
            .Select(ci =>
            {
                var product = ci.GetProduct() ?? _catalogueService.GetProductById(ci.GetProductId());

                if (product != null)
                {
                    ci.SetProduct(product);
                }

                decimal itemPrice = product?.Productdetail?.GetPrice()
                                    ?? product?.GetPrice()
                                    ?? 0m;

                return new CartDisplayItem
                {
                    ProductId = ci.GetProductId(),
                    ProductName = product?.GetProductName() ?? $"Product {ci.GetProductId()}",
                    Quantity = ci.GetQuantity(),
                    IsSelected = ci.IsSelected(),
                    IsObtainable = true,
                    AvailableQuantity = null,
                    RentalDays = rentalDays,
                    CartItemPrice = itemPrice,
                    Subtotal = 0m
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

        if (end < start)
        {
            return 0;
        }

        return Math.Max(1, (end.Date - start.Date).Days + 1);
    }
}