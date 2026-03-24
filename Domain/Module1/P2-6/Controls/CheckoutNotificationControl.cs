/*
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutNotificationControl
{
    private readonly INotificationService _notifSvc;
    private readonly ICheckoutMapper _checkoutMapper;

    public CheckoutNotificationControl(
        INotificationService notifSvc,
        ICheckoutMapper checkoutMapper)
    {
        _notifSvc = notifSvc;
        _checkoutMapper = checkoutMapper;
    }

    public void SetOrderNotificationOptIn(int checkoutId, bool optIn)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        checkout.SetNotifyOptIn(optIn);
        _checkoutMapper.Update(checkout);
    }

    public void SendOrderConfirmation(string orderId)
    {
    }
}
*/