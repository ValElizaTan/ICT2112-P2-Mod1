using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using Team6 = ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module1.P24.Controls;

public class StaffDashboardControl
{
    private readonly Team6.IInventoryService _inventoryService;
    private readonly ICustomerService _customerService;
    private readonly IOrderTrackingService _orderTrackingService;
    public StaffDashboardControl(
        Team6.IInventoryService inventoryService,
        ICustomerService customerService,
        IOrderTrackingService orderTrackingService)
    {        
        _orderTrackingService = orderTrackingService;
        _inventoryService = inventoryService;
        _customerService = customerService;
    }

    // GET all orders for staff dashboard 
    public List<Order> GetAllOrders()
    {
        return _orderTrackingService.GetAllOrders() ?? new List<Order>();
    }

    // GET all products for staff dashboard 
    public List<Product> GetAllProducts()
    {
        return _inventoryService.GetAllProducts() ?? new List<Product>();
    }

    // GET all inventory items by status for staff dashboard 
    public List<Inventoryitem> GetInventoryItemsByStatus(InventoryStatus status)
    {
        return _inventoryService.GetInventoryItemByStatus(status) ?? new List<Inventoryitem>();
    }

    // GET customer name by ID for staff dashboard
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

    // Update status of an order to "Ready for Dispatch"
    public void NotifyReadyForDispatch(int orderId,int staffId)
    {
        _orderTrackingService.UpdateStatus(orderId, OrderStatus.READY_FOR_DISPATCH, "Order is ready for dispatch", staffId);
    }
}
