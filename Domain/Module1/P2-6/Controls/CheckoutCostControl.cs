using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutCostControl
{
    private readonly ICostCalculation _costCalculation;
    private readonly ICatalogueService _catalogueService;
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly ICartService _cartService;

    public CheckoutCostControl(
        ICostCalculation costCalculation,
        ICatalogueService catalogueService,
        ICheckoutMapper checkoutMapper,
        ICartService cartService)
    {
        _costCalculation = costCalculation;
        _catalogueService = catalogueService;
        _checkoutMapper = checkoutMapper;
        _cartService = cartService;
    }

    public CostSummary GetCostSummary(int checkoutId)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        var cart = _cartService.GetCart(checkout.GetCartId());
        var selectedCartItems = cart.GetItems()
            .Where(x => x.IsSelected())
            .ToList();

        if (!selectedCartItems.Any())
        {
            return new CostSummary();
        }

        int rentalDays = GetRentalDays(cart);
        if (rentalDays <= 0)
        {
            return new CostSummary();
        }

        var selectedItems = new List<SelectedItem>();

        foreach (var cartItem in selectedCartItems)
        {
            try
            {
                var product = cartItem.GetProduct() ?? _catalogueService.GetProductById(cartItem.GetProductId());
                if (product != null)
                {
                    selectedItems.Add(new SelectedItem(product, cartItem.GetQuantity()));
                }
            }
            catch
            {
                // skip invalid product
            }
        }

        if (!selectedItems.Any())
        {
            return new CostSummary();
        }

        int shippingOptionId = checkout.GetShippingOptionId() ?? 0;

        var summary = _costCalculation.CalculateFinalOrderCost(
            selectedItems,
            rentalDays,
            shippingOptionId
        );

        summary.UnobtainableItems = GetUnobtainableItems(checkoutId);
        return summary;
    }

    public List<UnobtainableItemInfo> GetUnobtainableItems(int checkoutId)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        var cart = _cartService.GetCart(checkout.GetCartId());
        var selectedItems = cart.GetItems()
            .Where(x => x.IsSelected())
            .ToList();

        var unobtainableItems = new List<UnobtainableItemInfo>();

        foreach (var item in selectedItems)
        {
            try
            {
                var availability = _catalogueService.GetProductAvailability(item.GetProductId());

                if (!availability.IsAvailable)
                {
                    unobtainableItems.Add(new UnobtainableItemInfo());
                }
            }
            catch
            {
                unobtainableItems.Add(new UnobtainableItemInfo());
            }
        }

        return unobtainableItems;
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