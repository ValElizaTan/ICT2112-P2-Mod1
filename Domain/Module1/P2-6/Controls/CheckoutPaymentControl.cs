using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Controls;

public class CheckoutPaymentControl
{
    private readonly ICheckoutMapper _checkoutMapper;

    // REAL FUTURE PAYMENT FLOW
    // Uncomment when payment feature is ready
    // private readonly IPaymentGatewayService _paymentGateway;

    public CheckoutPaymentControl(
        ICheckoutMapper checkoutMapper
        // , IPaymentGatewayService paymentGateway
    )
    {
        _checkoutMapper = checkoutMapper;
        // _paymentGateway = paymentGateway;
    }

    public void SubmitPaymentDetails(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        if (string.IsNullOrWhiteSpace(nameOnCard) ||
            string.IsNullOrWhiteSpace(cardNumber) ||
            string.IsNullOrWhiteSpace(expirationDate) ||
            string.IsNullOrWhiteSpace(securityCode))
        {
            throw new InvalidOperationException("Please enter all payment details.");
        }

        if (cardNumber.Length < 12 || cardNumber.Length > 19)
        {
            throw new InvalidOperationException("Card number is invalid.");
        }

        if (securityCode.Length < 3 || securityCode.Length > 4)
        {
            throw new InvalidOperationException("Security code is invalid.");
        }

        if (!DateOnly.TryParse(expirationDate, out var expiry))
        {
            throw new InvalidOperationException("Expiration date is invalid.");
        }

        if (expiry < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new InvalidOperationException("Card has expired.");
        }

        // =========================
        // REAL FUTURE PAYMENT FLOW
        // Uncomment when payment feature is ready
        // =========================
        /*
        var details = new CreditCardPaymentDetails(
            cardNumber,
            expiry,
            int.Parse(securityCode),
            nameOnCard
        );

        checkout.SetPaymentMethodType(details.GetMethodType());
        _checkoutMapper.Update(checkout);
        */
    }

    public bool ProcessPayment(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        // =========================
        // ACTIVE TEST FLOW
        // simulated payment logic for now
        // =========================
        if (cardNumber == "0000000000000000")
        {
            throw new InvalidOperationException("Simulated payment failure.");
        }

        if (securityCode == "000")
        {
            throw new InvalidOperationException("Simulated payment rejected.");
        }

        return true;
    }

    // =========================
    // REAL FUTURE PAYMENT FLOW
    // Uncomment when payment feature is ready
    // =========================
    /*
    public Transaction ProcessPayment(
        int checkoutId,
        int orderId,
        decimal totalAmount,
        decimal depositAmount,
        PaymentMethodDetails details)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        var transaction = _paymentGateway.MakePayment(
            orderId,
            totalAmount,
            depositAmount,
            TransactionPurpose.DEPOSIT,
            details
        );

        if (transaction == null)
        {
            throw new InvalidOperationException("Payment transaction was not created.");
        }

        return transaction;
    }
    */
}