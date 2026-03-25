using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutLifecycleControl
{
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly ICartService _cartService;
    private readonly ICustomerValidationService _customerValidationService;

    // Uncomment when merge with ICustomer Service
    private readonly ICustomerService _customerService;

    public CheckoutLifecycleControl(
        ICheckoutMapper checkoutMapper,
        ICartService cartService,
        ICustomerValidationService customerValidationService,
        // Uncomment when merge with ICustomer Service
        ICustomerService customerService)
    {
        _checkoutMapper = checkoutMapper;
        _cartService = cartService;
        _customerValidationService = customerValidationService;
        // Uncomment when merge with ICustomer Service
        _customerService = customerService;
    }

    public Checkout InitCheckout(int customerId, List<int> productIds)
    {
        // TEMP DISABLED
        // var validation = _customerValidationService.ValidateCustomer(customerId);
        // if (!validation.IsValid)
        // {
        //     throw new InvalidOperationException(
        //         validation.ValidationMessage ?? "Customer is not allowed to checkout.");
        // }

        int cartId = _cartService.GetOrCreateActiveCartIdByCustomerId(customerId);
        var cart = _cartService.GetCart(cartId);

        if (cart == null)
        {
            throw new InvalidOperationException("Cart not found.");
        }

        var selectedItems = cart.GetItems()
            .Where(x => x.IsSelected() && productIds.Contains(x.GetProductId()))
            .ToList();

        if (!selectedItems.Any())
        {
            throw new InvalidOperationException("No selected items found for checkout.");
        }

        var existing = _checkoutMapper.FindActiveByCustomerId(customerId);
        if (existing != null)
        {
            return existing;
        }

        var checkout = new Checkout();
        checkout.Initialize(customerId, cartId);

        _checkoutMapper.Insert(checkout);
        return checkout;
    }

    public Checkout GetCheckout(int checkoutId)
    {
        return _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");
    }

    public Cart GetCartSnapshot(int checkoutId)
    {
        var checkout = GetCheckout(checkoutId);
        return _cartService.GetCart(checkout.GetCartId());
    }

    // Uncomment when merge with ICustomerService
    
    public Customer LoadCustomerInfo(int checkoutId)
    {
        var checkout = GetCheckout(checkoutId);
        return _customerService.GetCustomerInformation(checkout.GetCustomerId());
    }

    public List<string> ValidateCheckout(int checkoutId)
    {
        var warnings = new List<string>();
        var checkout = GetCheckout(checkoutId);
        var cart = _cartService.GetCart(checkout.GetCartId());

        // TEMP DISABLED
        // if (checkout.GetInternalStatus() != CheckoutStatus.IN_PROGRESS)
        // {
        //     warnings.Add("Checkout is not in progress.");
        // }

        if (cart == null || cart.IsEmpty())
        {
            warnings.Add("Cart is empty.");
            return warnings;
        }

        if (cart.GetRentalStart() == DateTime.MinValue || cart.GetRentalEnd() == DateTime.MinValue)
        {
            warnings.Add("Rental period is not set.");
        }

        if (cart.GetRentalEnd() < cart.GetRentalStart())
        {
            warnings.Add("Rental end date cannot be earlier than rental start date.");
        }

        if (!cart.GetItems().Any(x => x.IsSelected()))
        {
            warnings.Add("Please select at least one item.");
        }

        // TEMP DISABLED
        // if (string.IsNullOrWhiteSpace(checkout.GetShippingOptionId()))
        // {
        //     warnings.Add("Please select a delivery method.");
        // }

        return warnings;
    }

    public string ConfirmCheckout(int checkoutId)
    {
        var checkout = GetCheckout(checkoutId);
        checkout.MarkConfirmed();
        _checkoutMapper.Update(checkout);
        return checkout.GetCheckoutId().ToString();
    }

    public void CancelCheckout(int checkoutId)
    {
        var checkout = GetCheckout(checkoutId);
        checkout.MarkCancelled();
        _checkoutMapper.Update(checkout);
    }
}