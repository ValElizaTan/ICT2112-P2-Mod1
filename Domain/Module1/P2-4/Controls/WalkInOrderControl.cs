using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Interfaces.Data;
using Team6 = ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module1.P24.Controls;

public class WalkInOrderControl
{
    private readonly Team6.IOrderService _orderService;
    private readonly Team6.IInventoryService _inventoryService;
    private readonly ICartMapper _cartMapper;
    private readonly ICheckoutMapper _checkoutMapper;
    private readonly ICustomerService? _customerService;

    private Customer? _capturedCustomer;
    private int _staffId;

    public WalkInOrderControl(
        Team6.IOrderService orderService,
        Team6.IInventoryService inventoryService,
        ICartMapper cartMapper,
        ICheckoutMapper checkoutMapper,
        ICustomerService? customerService = null)
    {
        _orderService = orderService;
        _inventoryService = inventoryService;
        _cartMapper = cartMapper;
        _checkoutMapper = checkoutMapper;
        _customerService = customerService;
    }

    public void StartWalkInOrder(int staffId, int customerId)
    {
        _staffId = staffId;
        _capturedCustomer = _customerService?.GetCustomerInformation(customerId);
    }

    public void CaptureCustomerDetails(Customer customer)
    {
        _capturedCustomer = customer;
    }

    public Order CreateOrder(int customerId, List<int> productIds, DeliveryType deliveryType)
    {
        var rentalStart = DateTime.UtcNow;
        var deliveryDuration = MapDeliveryDuration(deliveryType);
        var rentalEnd = rentalStart.AddDays(GetRentalDays(deliveryDuration));

        // 1. Create a walk-in cart
        var cart = new Cart();
        cart.SetCustomerId(customerId);
        cart.MarkActive();
        cart.SetRentalPeriod(rentalStart, rentalEnd);
        _cartMapper.Insert(cart);

        // 2. Create a walk-in checkout linked to the cart
        var checkout = new Checkout();
        checkout.Initialize(customerId, cart.GetCartId());
        checkout.MarkConfirmed();
        _checkoutMapper.Insert(checkout);

        // 3. Build item data from product IDs (quantity 1 each for walk-in)
        var itemData = new List<(int productId, int quantity, decimal unitPrice, DateTime rentalStart, DateTime rentalEnd)>();
        var productQuantities = new Dictionary<int, int>();

        foreach (var pid in productIds)
        {
            var product = _inventoryService.GetProduct(pid);
            var unitPrice = product?.GetPrice() ?? 0m;

            itemData.Add((pid, 1, unitPrice, rentalStart, rentalEnd));
            productQuantities[pid] = 1;
        }

        var totalAmount = itemData.Sum(i => i.unitPrice * i.quantity);

        // 4. Create the order with the real checkout ID
        return _orderService.CreateOrder(
            customerId,
            checkout.GetCheckoutId(),
            itemData,
            deliveryDuration,
            totalAmount,
            productQuantities);
    }

    private static DeliveryDuration MapDeliveryDuration(DeliveryType type) => type switch
    {
        DeliveryType.EXPRESS     => DeliveryDuration.NextDay,
        DeliveryType.STANDARD    => DeliveryDuration.OneWeek,
        DeliveryType.SELF_PICKUP => DeliveryDuration.NextDay,
        _                        => DeliveryDuration.OneWeek
    };

    private static int GetRentalDays(DeliveryDuration duration) => duration switch
    {
        DeliveryDuration.NextDay   => 1,
        DeliveryDuration.ThreeDays => 3,
        DeliveryDuration.OneWeek   => 7,
        _                          => 7
    };
}
