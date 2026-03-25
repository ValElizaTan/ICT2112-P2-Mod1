using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Interfaces.Data;

namespace ProRental.Domain.Controls;

public class CheckoutNotificationControl
{
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly INotificationPreferenceService _notificationPreferenceService;
    private readonly ICustomerService _customerService;

    public CheckoutNotificationControl(
        ICheckoutMapper checkoutMapper,
        INotificationPreferenceService notificationPreferenceService,
        ICustomerService customerService)
    {
        _checkoutMapper = checkoutMapper;
        _notificationPreferenceService = notificationPreferenceService;
        _customerService = customerService;
    }

    public void SetOrderNotificationOptIn(int checkoutId, bool optIn)
    {
        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        checkout.SetNotifyOptIn(optIn);
        _checkoutMapper.Update(checkout);

        var customer = _customerService.GetCustomerInformation(checkout.GetCustomerId())
            ?? throw new InvalidOperationException("Customer information not found.");

        var customerInfo = customer.GetCustomerInfo();
        var user = customerInfo?.User
            ?? throw new InvalidOperationException("User information not found.");

        int userId = user.UserId;

        var existingPreference = _notificationPreferenceService.GetPreference(userId);

        if (existingPreference == null)
        {
            var newPreference = new Notificationpreference(
                0,
                userId,
                NotificationFrequency.DAILY,
                NotificationGranularity.ALL,
                optIn,
                false
            );

            _notificationPreferenceService.SetPreference(newPreference);
        }
        else
        {
            var existingInfo = existingPreference.GetNotificationPreferenceInfo();

            var updatedInfo = new NotificationPreferenceInfo(
                existingInfo.PreferenceId,
                existingInfo.UserId,
                existingInfo.NotificationFrequency,
                existingInfo.NotificationGranularity,
                optIn,
                existingInfo.SmsEnabled
            );

            existingPreference.SetNotificationPreferenceInfo(updatedInfo);
            _notificationPreferenceService.SetPreference(existingPreference);
        }
    }
}