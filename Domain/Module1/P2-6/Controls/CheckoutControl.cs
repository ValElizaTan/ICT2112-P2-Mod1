using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutControl : ICheckoutService
{
    private readonly CheckoutLifecycleControl _lifecycleCtrl;
    private readonly CheckoutShippingControl _shippingCtrl;
    private readonly CheckoutPaymentControl _paymentCtrl;
    private readonly CheckoutCostControl _costCtrl;

    public CheckoutControl(
        CheckoutLifecycleControl lifecycleCtrl,
        CheckoutShippingControl shippingCtrl,
        CheckoutPaymentControl paymentCtrl,
        CheckoutCostControl costCtrl)
    {
        _lifecycleCtrl = lifecycleCtrl;
        _shippingCtrl = shippingCtrl;
        _paymentCtrl = paymentCtrl;
        _costCtrl = costCtrl;
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

        /*
        // =========================
        // SIMULATED PAYMENT VERSION
        // =========================
        bool paymentSuccessful = !string.IsNullOrWhiteSpace(cardNumber)
                                 && cardNumber.Replace(" ", "").Length >= 12;

        if (!paymentSuccessful)
        {
            throw new InvalidOperationException("Payment failed. Please check your card details.");
        }
        */

        _paymentCtrl.ProcessPayment(
            checkoutId,
            amountToCharge,
            nameOnCard,
            cardNumber,
            expirationDate,
            securityCode
        );

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
}