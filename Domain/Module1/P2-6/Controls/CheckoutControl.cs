using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutControl : ICheckoutService
{
    private readonly CheckoutLifecycleControl _lifecycleCtrl;
    private readonly CheckoutShippingControl _shippingCtrl;
    private readonly CheckoutPaymentControl _paymentCtrl;
    private readonly CheckoutCostControl _costCtrl;
    private readonly CheckoutNotificationControl _notifCtrl;
    private readonly IOrderService _orderService;
    private readonly OrderBuilderControl _orderBuilderCtrl;

    public CheckoutControl(
        CheckoutLifecycleControl lifecycleCtrl,
        CheckoutShippingControl shippingCtrl,
        CheckoutPaymentControl paymentCtrl,
        CheckoutCostControl costCtrl,
        CheckoutNotificationControl notifCtrl,
        IOrderService orderService,
        OrderBuilderControl orderBuilderCtrl)
    {
        _lifecycleCtrl = lifecycleCtrl;
        _shippingCtrl = shippingCtrl;
        _paymentCtrl = paymentCtrl;
        _costCtrl = costCtrl;
        _notifCtrl = notifCtrl;
        _orderService = orderService;
        _orderBuilderCtrl = orderBuilderCtrl;
    }

    public string StartCheckout(int customerId, List<int> selectedProductIds)
    {
        var checkout = _lifecycleCtrl.InitCheckout(customerId, selectedProductIds);
        return checkout.GetCheckoutId().ToString();
    }

    public Checkout GetCheckout(int checkoutId)
    {
        return _lifecycleCtrl.GetCheckout(checkoutId);
    }

    public Customer LoadCustomerInfo(int checkoutId)
    {
        return _lifecycleCtrl.LoadCustomerInfo(checkoutId);
    }

    public List<ShippingOption> GetShippingOptions(int checkoutId)
    {
        return _shippingCtrl.GetShippingOptions(checkoutId);
    }

    public void SelectShippingOption(int checkoutId, int optionId)
    {
        _shippingCtrl.SelectShippingOption(checkoutId, optionId);
    }

    public void SetOrderNotificationOptIn(int checkoutId, bool optIn)
    {
        _notifCtrl.SetOrderNotificationOptIn(checkoutId, optIn);
    }

    public List<string> ValidateCheckout(int checkoutId)
    {
        var warnings = _lifecycleCtrl.ValidateCheckout(checkoutId);
        return warnings.Distinct().ToList();
    }

    public CostSummary GetCostSummary(int checkoutId)
    {
        return _costCtrl.GetCostSummary(checkoutId);
    }

    public string ConfirmCheckout(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        var warnings = ValidateCheckout(checkoutId);
        if (warnings.Any())
        {
            throw new InvalidOperationException(string.Join(" ", warnings));
        }

        var checkout = _lifecycleCtrl.GetCheckout(checkoutId);
        var cart = _lifecycleCtrl.GetCartSnapshot(checkoutId);

        if (checkout.GetShippingOptionId() == null)
        {
            throw new InvalidOperationException("Please select a shipping option before confirming checkout.");
        }

        _paymentCtrl.SubmitPaymentDetails(
            checkoutId,
            nameOnCard,
            cardNumber,
            expirationDate,
            securityCode
        );

        var costSummary = _costCtrl.GetCostSummary(checkoutId);
        var amountToCharge = costSummary.FinalOrderCost > 0
            ? costSummary.FinalOrderCost
            : costSummary.TotalCost;

        if (amountToCharge <= 0)
        {
            throw new InvalidOperationException("Total payment amount is invalid.");
        }

        _paymentCtrl.ProcessPayment(
            checkoutId,
            amountToCharge,
            nameOnCard,
            cardNumber,
            expirationDate,
            securityCode
        );

        var selectedShippingOption = _shippingCtrl.GetSelectedShippingOption(checkoutId);
        var deliveryType = _orderBuilderCtrl.ResolveDeliveryDuration(selectedShippingOption);

        var itemData = _orderBuilderCtrl.BuildOrderItems(cart);
        if (!itemData.Any())
        {
            throw new InvalidOperationException("No selected items found to create the order.");
        }

        var productQuantities = _orderBuilderCtrl.BuildProductQuantities(cart);

        var order = _orderService.CreateOrder(
            checkout.GetCustomerId(),
            checkoutId,
            itemData,
            deliveryType,
            amountToCharge,
            productQuantities
        );

        _lifecycleCtrl.ConfirmCheckout(checkoutId);

        return order.OrderId.ToString();
    }

    public void CancelCheckout(int checkoutId)
    {
        _lifecycleCtrl.CancelCheckout(checkoutId);
    }

    public Cart GetSelectedCartSnapshot(int checkoutId)
    {
        return _lifecycleCtrl.GetCartSnapshot(checkoutId);
    }
}