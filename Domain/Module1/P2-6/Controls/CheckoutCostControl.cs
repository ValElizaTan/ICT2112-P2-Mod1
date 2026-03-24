using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutCostControl
{
    private readonly ICostCalculation _costCalculation;
    private readonly IInventoryService _inventorySvc;
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly ICartService _cartService;
    private readonly AppDbContext _context;

    public CheckoutCostControl(
        ICostCalculation costCalculation,
        IInventoryService inventorySvc,
        ICheckoutMapper checkoutMapper,
        ICartService cartService,
        AppDbContext context)
    {
        _costCalculation = costCalculation;
        _inventorySvc = inventorySvc;
        _checkoutMapper = checkoutMapper;
        _cartService = cartService;
        _context = context;
    }

    public CostSummary GetCostSummary(int checkoutId)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        var summary = _cartService.GetCartDisplaySummary(checkout.GetCartId());

        var optionId = checkout.GetShippingOptionId();

        if (optionId.HasValue)
        {
            var shippingOption = _context.Set<ShippingOption>()
                .FirstOrDefault(x => EF.Property<int?>(x, "OptionId") == optionId.Value);

            if (shippingOption != null)
            {
                summary.DeliveryCost = shippingOption.GetCost();
            }
        }

        summary.FinalOrderCost =
            summary.RentalCost + summary.DepositAmount + summary.DeliveryCost;

        return summary;
    }

    public List<UnobtainableItemInfo> GetUnobtainableItems(int checkoutId)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        var cart = _cartService.GetCart(checkout.GetCartId());

        // Replace this with real inventory-based logic when UnobtainableItemInfo is ready.
        return new List<UnobtainableItemInfo>();
    }

    public decimal GetDepositAmount(int checkoutId)
    {
        return GetCostSummary(checkoutId).DepositAmount;
    }
}