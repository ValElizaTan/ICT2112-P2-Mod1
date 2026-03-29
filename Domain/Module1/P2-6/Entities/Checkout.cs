using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Checkout
{
    private CheckoutStatus? _status;
    private CheckoutStatus? Status { get => _status; set => _status = value; }

    private PaymentMethod? _paymentMethodType;
    private PaymentMethod? PaymentMethodType
    {
        get => _paymentMethodType;
        set => _paymentMethodType = value;
    }

    public int GetCheckoutId() => Checkoutid;
    public int GetCustomerId() => Customerid;
    public int GetCartId() => Cartid;

    public void SetShippingOption(int optionId)
    {
        this.OptionId = optionId;
    }

    public int? GetShippingOptionId()
    {
        return this.OptionId;
    }

    // TEMP DISABLED
    /*
    public void SetPaymentMethodType(PaymentMethod methodType)
    {
        UpdatePaymentMethodType(methodType);
    }

    public PaymentMethod GetPaymentMethodType()
    {
        return PaymentMethodType ?? PaymentMethod.CREDIT_CARD;
    }
    */

    public void MarkConfirmed()
    {
        Status = CheckoutStatus.CONFIRMED;
    }

    public void MarkCancelled()
    {
        Status = CheckoutStatus.CANCELLED;
    }

    public void SetNotifyOptIn(bool optIn)
    {
        Notifyoptin = optIn;
    }

    public bool GetNotifyOptIn()
    {
        return Notifyoptin ?? false;
    }

    internal void Initialize(int customerId, int cartId)
    {
        Customerid = customerId;
        Cartid = cartId;
        Createdat = DateTime.UtcNow;
        Notifyoptin = false;
        OptionId = null;
        Status = CheckoutStatus.IN_PROGRESS;
        PaymentMethodType = PaymentMethod.CREDIT_CARD;
    }

    internal CheckoutStatus GetInternalStatus()
    {
        return Status ?? CheckoutStatus.IN_PROGRESS;
    }

    internal bool GetInternalNotifyOptIn()
    {
        return Notifyoptin ?? false;
    }
}