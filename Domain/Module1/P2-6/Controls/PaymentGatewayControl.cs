namespace ProRental.Domain.Controls;

using ProRental.Data;

using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

public class PaymentGatewayControl : IPaymentGatewayService
{
    private readonly IPaymentAdaptorSelector _paymentAdaptorSelector;

    public PaymentGatewayControl(IPaymentAdaptorSelector paymentAdaptorSelector)
    {
        _paymentAdaptorSelector = paymentAdaptorSelector;
    }

    public TransactionResponse MakePayment(int orderId, decimal totalAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails)
    {
        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(totalAmount, purpose, paymentMethodDetails);
        var response = paymentAdaptor.ProcessPayment(orderId, totalAmount, paymentMethodDetails);
        response.ProviderName = GetProviderName(paymentAdaptor);
        return response;
    }

    public TransactionResponse GetRefundDeposit(int refundId)
    {
        const decimal refundAmount = 100m;
        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(refundAmount, TransactionPurpose.REFUND_DEPOSIT, paymentMethodDetails: null);
        var response = paymentAdaptor.ProcessRefund(refundId, refundAmount, paymentMethodDetails: null);
        response.ProviderName = GetProviderName(paymentAdaptor);
        return response;
    }

    public TransactionResponse MakePenaltyPayment(int orderId, decimal penaltyAmount, TransactionPurpose purpose, PaymentMethodDetails paymentMethodDetails)
    {
        var paymentAdaptor = _paymentAdaptorSelector.SelectAdaptor(penaltyAmount, purpose, paymentMethodDetails);
        var response = paymentAdaptor.ProcessPayment(orderId, penaltyAmount, paymentMethodDetails);
        response.ProviderName = GetProviderName(paymentAdaptor);
        return response;
    }

    private static string GetProviderName(IPaymentAdaptors adaptor)
    {
        return adaptor.GetType().Name.Replace("Adapter", string.Empty);
    }
}
