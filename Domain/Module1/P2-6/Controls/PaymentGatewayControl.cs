namespace ProRental.Domain.Controls;

using ProRental.Data;

using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

public class PaymentGatewayControl : IPaymentGatewayService
{
    private readonly IPaymentAdaptorSelector _paymentAdaptorSelector;
    private readonly ITransactionMapper _transactionMapper;

    public PaymentGatewayControl(IPaymentAdaptorSelector paymentAdaptorSelector, ITransactionMapper transactionMapper)
    {
        _paymentAdaptorSelector = paymentAdaptorSelector;
        _transactionMapper = transactionMapper;
    }

    public TransactionResponse MakePayment(decimal totalAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails)
    {
        var transaction = Transaction.Create(
            totalAmount,
            TransactionType.PAYMENT,
            purpose,
            providerTransactionId: null,
            status: TransactionStatus.PENDING
        );
        var transactionId = _transactionMapper.Insert(transaction);

        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(totalAmount, purpose, paymentMethodDetails);
        var response = paymentAdaptor.ProcessPayment(transactionId, totalAmount, paymentMethodDetails);
        response.providerName = GetProviderName(paymentAdaptor);

        transaction.ApplyProviderResult(response.providerTransactionId, response.status);
        _transactionMapper.Update(transaction);

        response.transactionId = transactionId;
        return response;
    }

    public TransactionResponse GetRefundDeposit(int refundId)
    {
        const decimal refundAmount = 100m;
        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(refundAmount, TransactionPurpose.REFUND_DEPOSIT, paymentMethodDetails: null);
        var transaction = Transaction.Create(
            refundAmount,
            TransactionType.REFUND,
            TransactionPurpose.REFUND_DEPOSIT,
            providerTransactionId: null,
            status: TransactionStatus.PENDING
        );
        var transactionId = _transactionMapper.Insert(transaction);

        var response = paymentAdaptor.ProcessRefund(transactionId, refundAmount, paymentMethodDetails: null);
        response.providerName = GetProviderName(paymentAdaptor);

        transaction.ApplyProviderResult(response.providerTransactionId, response.status);
        _transactionMapper.Update(transaction);

        response.transactionId = transactionId;
        return response;
    }

    public TransactionResponse MakePenaltyPayment(int orderId, decimal penaltyAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails)
    {
        var transaction = Transaction.Create(
            penaltyAmount,
            TransactionType.PAYMENT,
            purpose,
            providerTransactionId: null,
            status: TransactionStatus.PENDING
        );
        var transactionId = _transactionMapper.Insert(transaction);

        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(penaltyAmount, purpose, paymentMethodDetails);
        var response = paymentAdaptor.ProcessPayment(transactionId, penaltyAmount, paymentMethodDetails);
        response.providerName = GetProviderName(paymentAdaptor);

        transaction.ApplyProviderResult(response.providerTransactionId, response.status);
        _transactionMapper.Update(transaction);

        response.transactionId = transactionId;
        return response;
    }

    private static string GetProviderName(IPaymentAdaptors adaptor)
    {
        return adaptor.GetType().Name.Replace("Adapter", string.Empty);
    }
}
