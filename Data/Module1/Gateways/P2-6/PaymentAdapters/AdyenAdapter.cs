using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data;

public class AdyenAdapter : IPaymentAdaptors
{
    private readonly IPaymentProviderClient _paymentProviderClient;

    public AdyenAdapter(IPaymentProviderClient paymentProviderClient)
    {
        _paymentProviderClient = paymentProviderClient;
    }

    public int Priority => 30;

    public bool CanHandle(decimal amount, TransactionPurpose purpose, PaymentMethodDetails? paymentMethodDetails)
    {
        return purpose != TransactionPurpose.REFUND_DEPOSIT && amount >= 1000m;
    }

    public TransactionResponse ProcessPayment(int transactionId, decimal amount, PaymentMethodDetails? paymentMethodDetails)
    {
        var result = _paymentProviderClient.Charge(amount);
        var status = result.Success ? TransactionStatus.COMPLETED : TransactionStatus.FAILED;
        return new TransactionResponse(transactionId, result.ProviderTransactionId, status, result.Message);
    }

    public TransactionResponse ProcessRefund(int transactionId, decimal amount, PaymentMethodDetails? paymentMethodDetails)
    {
        var result = _paymentProviderClient.Refund(amount);
        var status = result.Success ? TransactionStatus.COMPLETED : TransactionStatus.FAILED;
        return new TransactionResponse(transactionId, result.ProviderTransactionId, status, result.Message);
    }

    public void UpdatePaymentStatus(int transactionId, TransactionStatus status)
    {
        // Mock adapter: no-op
    }

    public void UpdateRefundStatus(int transactionId, TransactionStatus status)
    {
        // Mock adapter: no-op
    }
}
