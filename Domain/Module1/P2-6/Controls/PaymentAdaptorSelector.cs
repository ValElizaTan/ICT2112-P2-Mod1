using System;
using System.Collections.Generic;
using System.Linq;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Controls;

public class PaymentAdaptorSelector : IPaymentAdaptorSelector
{
    private readonly IEnumerable<IPaymentAdaptors> _paymentAdaptors;

    public PaymentAdaptorSelector(IEnumerable<IPaymentAdaptors> paymentAdaptors)
    {
        _paymentAdaptors = paymentAdaptors ?? throw new ArgumentNullException(nameof(paymentAdaptors));
    }

    public IPaymentAdaptors SelectAdaptor(decimal amount, TransactionPurpose purpose, PaymentMethodDetails? paymentMethodDetails)
    {
        var adaptor = _paymentAdaptors
            .Where(a => a.CanHandle(amount, purpose, paymentMethodDetails))
            .OrderByDescending(a => a.priority)
            .FirstOrDefault();

        if (adaptor != null)
        {
            return adaptor;
        }

        var fallback = _paymentAdaptors.OrderByDescending(a => a.priority).FirstOrDefault();
        if (fallback == null)
        {
            throw new InvalidOperationException("No payment adaptors registered.");
        }

        return fallback;
    }
}
