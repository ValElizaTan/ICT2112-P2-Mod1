using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutControl : ICheckoutService
{
    private readonly CheckoutLifecycleControl _lifecycleCtrl;
    private readonly CheckoutShippingControl _shippingCtrl;
    private readonly CheckoutPaymentControl _paymentCtrl;
    private readonly CheckoutCostControl _costCtrl;


    // TEMP DISABLED / INCOMPLETE
    // private readonly CheckoutCarbonControl _carbonCtrl;
    // private readonly CheckoutCostControl _costCtrl;
    // private readonly CheckoutNotificationControl _notifCtrl;
    // private readonly OrderBuilderControl _orderBuilderCtrl;
    // private readonly IOrderService _orderService;

    public CheckoutControl(
        CheckoutLifecycleControl lifecycleCtrl,
        CheckoutShippingControl shippingCtrl,
        CheckoutPaymentControl paymentCtrl,
        CheckoutCostControl costCtrl
        // , CheckoutCarbonControl carbonCtrl
        // , CheckoutNotificationControl notifCtrl
        // , OrderBuilderControl orderBuilderCtrl
        // , IOrderService orderService
    )
    {
        _lifecycleCtrl = lifecycleCtrl;
        _shippingCtrl = shippingCtrl;
        _paymentCtrl = paymentCtrl;
        _costCtrl = costCtrl;

        // _carbonCtrl = carbonCtrl;
        // _notifCtrl = notifCtrl;
        // _orderBuilderCtrl = orderBuilderCtrl;
        // _orderService = orderService;
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

    // Uncomment when merge with ICustomerService
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

    public List<string> ValidateCheckout(int checkoutId)
    {
        var warnings = _lifecycleCtrl.ValidateCheckout(checkoutId);

        // future extra validation
        // warnings.AddRange(_orderBuilderCtrl.ValidateBeforeConfirm(GetCheckout(checkoutId)));

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

        _paymentCtrl.SubmitPaymentDetails(
            checkoutId,
            nameOnCard,
            cardNumber,
            expirationDate,
            securityCode
        );

        // =========================
        // ACTIVE TEST FLOW
        // simulated payment now
        // =========================
        var paymentSuccess = _paymentCtrl.ProcessPayment(
            checkoutId,
            nameOnCard,
            cardNumber,
            expirationDate,
            securityCode
        );

        if (!paymentSuccess)
        {
            throw new InvalidOperationException("Payment failed.");
        }

        _lifecycleCtrl.ConfirmCheckout(checkoutId);
        return $"CHK-{checkoutId}";
    }

    public void CancelCheckout(int checkoutId)
    {
        _lifecycleCtrl.CancelCheckout(checkoutId);
    }

    public Cart GetSelectedCartSnapshot(int checkoutId)
    {
        return _lifecycleCtrl.GetCartSnapshot(checkoutId);
    }

    // =========================
    // REAL FUTURE PAYMENT FLOW
    // Uncomment when payment feature is ready
    // =========================
    /*
    public string ConfirmCheckout(int checkoutId, PaymentMethodDetails details)
    {
        var warnings = ValidateCheckout(checkoutId);
        if (warnings.Any())
        {
            throw new InvalidOperationException(string.Join(" ", warnings));
        }

        var checkout = GetCheckout(checkoutId);
        var cart = _lifecycleCtrl.GetCartSnapshot(checkoutId);
        var selectedOption = _shippingCtrl.GetSelectedShippingOption(checkoutId);

        var costSummary = _costCtrl.GetCostSummary(checkoutId);
        var depositAmount = _costCtrl.GetDepositAmount(checkoutId);

        var itemData = _orderBuilderCtrl.BuildOrderItems(cart);
        var productQuantities = _orderBuilderCtrl.BuildProductQuantities(cart);
        var deliveryType = _orderBuilderCtrl.ResolveDeliveryDuration(selectedOption);

        var order = _orderService.CreateOrder(
            customerId: checkout.GetCustomerId(),
            checkoutId: checkout.GetCheckoutId(),
            itemData: itemData,
            deliveryType: deliveryType,
            totalAmount: costSummary.FinalOrderCost,
            productQuantities: productQuantities
        );

        var transaction = _paymentCtrl.ProcessPayment(
            checkoutId,
            order.GetOrderId(),
            costSummary.FinalOrderCost,
            depositAmount,
            details
        );

        if (transaction == null)
        {
            throw new InvalidOperationException("Payment failed.");
        }

        // if (transaction.GetStatus() != TransactionStatus.COMPLETED)
        // {
        //     throw new InvalidOperationException("Payment failed.");
        // }

        _lifecycleCtrl.ConfirmCheckout(checkoutId);

        if (checkout.GetInternalNotifyOptIn())
        {
            _notifCtrl.SendOrderConfirmation(order.GetOrderId().ToString());
        }

        return $"ORD-{order.GetOrderId()}";
    }
    */
}