
using ProRental.Data;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

public interface IPaymentAdaptors
{
    int Priority { get; }
    bool CanHandle(decimal amount, TransactionPurpose purpose, PaymentMethodDetails? paymentMethodDetails);
    public TransactionResponse ProcessPayment(int transactionId, decimal amount, PaymentMethodDetails? paymentMethodDetails);
    public void UpdatePaymentStatus(int transactionId, TransactionStatus status);
    public TransactionResponse ProcessRefund(int transactionId, decimal amount, PaymentMethodDetails? paymentMethodDetails);
    public void UpdateRefundStatus(int transactionId, TransactionStatus status);
}
