using ProRental.Data;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Controls;

public class CheckoutPaymentControl
{
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly IPaymentGatewayService _paymentGateway;

    public CheckoutPaymentControl(
        ICheckoutMapper checkoutMapper,
        IPaymentGatewayService paymentGateway)
    {
        _checkoutMapper = checkoutMapper;
        _paymentGateway = paymentGateway;
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

        var normalizedCardNumber = cardNumber.Replace(" ", "").Replace("-", "");

        if (!normalizedCardNumber.All(char.IsDigit) ||
            normalizedCardNumber.Length < 12 ||
            normalizedCardNumber.Length > 19)
        {
            throw new InvalidOperationException("Card number is invalid.");
        }

        if (!securityCode.All(char.IsDigit) ||
            securityCode.Length < 3 ||
            securityCode.Length > 4)
        {
            throw new InvalidOperationException("Security code is invalid.");
        }

        if (!DateOnly.TryParse(expirationDate, out var expiry))
        {
            throw new InvalidOperationException("Expiration date is invalid.");
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (expiry < today)
        {
            throw new InvalidOperationException("Card has expired.");
        }

        _checkoutMapper.Update(checkout);
    }

    public TransactionResponse ProcessPayment(
        int paymentReferenceId,
        decimal amount,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException("Payment amount is invalid.");
        }

        var normalizedCardNumber = cardNumber.Replace(" ", "").Replace("-", "");

        if (!DateOnly.TryParse(expirationDate, out var expiry))
        {
            throw new InvalidOperationException("Expiration date is invalid.");
        }

        if (!int.TryParse(securityCode, out var parsedSecurityCode))
        {
            throw new InvalidOperationException("Security code is invalid.");
        }

        PaymentMethodDetails details = new CreditCardPaymentDetails(
            normalizedCardNumber,
            expiry,
            parsedSecurityCode,
            nameOnCard
        );

        var transaction = _paymentGateway.MakePayment(
            paymentReferenceId,
            amount,
            TransactionPurpose.ORDER,
            details
        );

        if (transaction == null)
        {
            throw new InvalidOperationException("Payment transaction was not created.");
        }

        if (transaction.status != TransactionStatus.COMPLETED)
        {
            throw new InvalidOperationException(
                string.IsNullOrWhiteSpace(transaction.message)
                    ? "Payment failed."
                    : transaction.message
            );
        }

        return transaction;
    }
}