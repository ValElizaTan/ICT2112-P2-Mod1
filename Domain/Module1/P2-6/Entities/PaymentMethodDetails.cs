namespace ProRental.Domain.Entities;
using ProRental.Domain.Enums;

public abstract class PaymentMethodDetails
{
    // Common properties for all payment methods can be defined here
    // For example, you might have a property for the payment method type
    protected PaymentMethod methodType { get; private set; }

    protected PaymentMethodDetails(PaymentMethod methodType)
    {
        this.methodType = methodType;
    }
}