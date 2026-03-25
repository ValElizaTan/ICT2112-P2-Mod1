using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class OrderBuilderControl
{
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly AppDbContext _context;

    public OrderBuilderControl(
        ICheckoutMapper checkoutMapper,
        AppDbContext context)
    {
        _checkoutMapper = checkoutMapper;
        _context = context;
    }

    public Cart BuildCartSnapshot(Cart fullCart, List<int> productIds)
    {
        return fullCart;
    }

    public List<(int productId, int quantity, decimal unitPrice, DateTime rentalStart, DateTime rentalEnd)> BuildOrderItems(Cart cart)
    {
        var rentalStart = cart.GetRentalStart();
        var rentalEnd = cart.GetRentalEnd();

        return cart.GetItems()
            .Where(x => x.IsSelected())
            .Select(x =>
            {
                var product = _context.Set<Product>()
                    .Include(p => p.Productdetail)
                    .First(p => EF.Property<int>(p, "Productid") == x.GetProductId());

                decimal unitPrice = product.Productdetail == null
                    ? 0m
                    : EF.Property<decimal>(product.Productdetail, "Price");

                return (
                    productId: x.GetProductId(),
                    quantity: x.GetQuantity(),
                    unitPrice: unitPrice,
                    rentalStart: rentalStart,
                    rentalEnd: rentalEnd
                );
            })
            .ToList();
    }

    public List<string> ValidateBeforeConfirm(Checkout checkout)
    {
        var warnings = new List<string>();

        // if (checkout.GetInternalStatus() != CheckoutStatus.IN_PROGRESS)
        // {
        //     warnings.Add("Checkout is not in progress.");
        // }

        // if (string.IsNullOrWhiteSpace(checkout.GetShippingOptionId()))
        // {
        //     warnings.Add("Shipping option has not been selected.");
        // }

        return warnings;
    }

    public Dictionary<int, int> BuildProductQuantities(Cart cart)
    {
        return cart.GetItems()
            .Where(x => x.IsSelected())
            .ToDictionary(x => x.GetProductId(), x => x.GetQuantity());
    }

    public DeliveryDuration ResolveDeliveryDuration(ShippingOption? selectedOption)
    {
        if (selectedOption == null)
        {
            return DeliveryDuration.OneWeek;
        }

        int days = EF.Property<int?>(selectedOption, "DeliveryDays") ?? 7;

        if (days <= 1) return DeliveryDuration.NextDay;
        if (days <= 3) return DeliveryDuration.ThreeDays;
        return DeliveryDuration.OneWeek;
    }
}