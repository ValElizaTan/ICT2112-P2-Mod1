using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class WalkInOrderControl
{
    private readonly IOrderService? _orderService;
    private readonly ICustomerService? _customerService;

    private Customer? _capturedCustomer;
    private int _staffId;

    public WalkInOrderControl(IOrderService? orderService = null, ICustomerService? customerService = null)
    {
        _orderService = orderService;
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

    public Order CreateOrder(int customerId, List<Orderitem> items, DeliveryType deliveryMethod)
    {
        if (_orderService == null)
            throw new InvalidOperationException("IOrderService is not registered. Awaiting Team 6 implementation.");
        return _orderService.CreateOrder(customerId, items, deliveryMethod);
    }
}
