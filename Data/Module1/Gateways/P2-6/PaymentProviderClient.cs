using System;

namespace ProRental.Data;

public record PaymentProviderResult(string ProviderTransactionId, bool Success, string Message);

public interface IPaymentProviderClient
{
    PaymentProviderResult Charge(decimal amount);
    PaymentProviderResult Refund(decimal amount);
}

public class MockPaymentProviderClient : IPaymentProviderClient
{
    public PaymentProviderResult Charge(decimal amount)
    {
        return new PaymentProviderResult(NewId("CHG"), true, $"Charge approved for {amount:C}.");
    }

    public PaymentProviderResult Refund(decimal amount)
    {
        return new PaymentProviderResult(NewId("RFD"), true, $"Mock refund approved for {amount:C}.");
    }

    private static string NewId(string prefix)
    {
        var token = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
        return $"{prefix}-{token}";
    }
}
