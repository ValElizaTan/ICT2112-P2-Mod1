using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using Team6 = ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module1.P24.Controls;

public class StaffDashboardControl
{
    private readonly Team6.IOrderService _orderService;
    private readonly Team6.IInventoryService _inventoryService;
    private readonly ICustomerService _customerService;

    public StaffDashboardControl(
        Team6.IOrderService orderService,
        Team6.IInventoryService inventoryService,
        ICustomerService customerService)
    {
        _orderService = orderService;
        _inventoryService = inventoryService;
        _customerService = customerService;
    }

    public List<Order> GetAllOrders()
    {
        return _orderService.GetOrders() ?? new List<Order>();
    }

    public List<Product> GetAllProducts()
    {
        return _inventoryService.GetAllProducts() ?? new List<Product>();
    }

    public List<Inventoryitem> GetInventoryItemsByStatus(InventoryStatus status)
    {
        return _inventoryService.GetInventoryItemByStatus(status) ?? new List<Inventoryitem>();
    }

    public string GetCustomerName(int customerId)
    {
        try
        {
            var customer = _customerService.GetCustomerInformation(customerId);
            var info = customer?.GetCustomerInfo();
            return info?.User?.Name ?? $"Customer #{customerId}";
        }
        catch
        {
            return $"Customer #{customerId}";
        }
    }

    public void NotifyReadyForDispatch(int orderId)
    {
        _orderService.UpdateOrderStatus(orderId, OrderStatus.CONFIRMED);
    }
}
