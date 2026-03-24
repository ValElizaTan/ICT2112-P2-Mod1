namespace ProRental.Interfaces.Domain;

using ProRental.Data;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;


public interface IPaymentGatewayService
{
    TransactionResponse MakePayment(int orderId, decimal totalAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails);
    TransactionResponse GetRefundDeposit(int refundId);
    TransactionResponse MakePenaltyPayment(int orderId, decimal penaltyAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails);
}
