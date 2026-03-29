using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

public interface IPaymentAdaptorSelector
{
    IPaymentAdaptors SelectAdaptor(decimal amount, TransactionPurpose purpose, PaymentMethodDetails? paymentMethodDetails);
}
