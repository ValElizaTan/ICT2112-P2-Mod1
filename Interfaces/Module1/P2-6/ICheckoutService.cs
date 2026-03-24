using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Domain;

public interface ICheckoutService
{
    string StartCheckout(int customerId, List<int> selectedProductIds);
    Checkout GetCheckout(int checkoutId);

    List<ShippingOption> GetShippingOptions(int checkoutId);
    void SelectShippingOption(int checkoutId, int optionId);

    List<string> ValidateCheckout(int checkoutId);

    // Real ConfirmCheckout function to use, uncomment when payment feature is ready
    // string ConfirmCheckout(int checkoutId, PaymentMethodDetails details);

    // ACTIVE TEST FLOW
    string ConfirmCheckout(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode);

    void CancelCheckout(int checkoutId);
    Cart GetSelectedCartSnapshot(int checkoutId);

    // TEMP DISABLED
    // List<UnobtainableItemInfo> GetUnobtainableItems(int checkoutId);
    Customer LoadCustomerInfo(int checkoutId);
    // CarbonResult GetDeliveryCarbonSummary(int checkoutId);
    // void SetOrderNotificationOptIn(int checkoutId, bool optIn);
    // CostSummary GetCostSummary(int checkoutId);
}