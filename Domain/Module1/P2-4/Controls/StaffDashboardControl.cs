using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Interfaces.Module2;

namespace ProRental.Domain.Module1.P24.Controls;

public class StaffDashboardControl
{
    private readonly IOrderService?          _orderService;
    private readonly IInventoryService?      _inventoryService;
    // private readonly IAuthenticationService? _authService;
    // private readonly ISessionService?        _sessionService;

    public StaffDashboardControl(
        IOrderService?          orderService         = null,
        IInventoryService?      inventoryService     = null)
        // IAuthenticationService? authService          = null
        // ISessionService?        sessionService       = null)
    {
        _orderService     = orderService;
        _inventoryService = inventoryService;
        // _authService      = authService;
        // _sessionService   = sessionService;
    }

    public List<Inventoryitem> GetInventoryItemsByStatus(InventoryStatus status)
    {
        return _inventoryService?.GetInventoryItemsByStatus(status) ?? new List<Inventoryitem>();
    }

    public void NotifyReadyForDispatch(int orderId)
    {
    }

    public List<Order> DisplayOrderList()
    {
        // return _orderService?.GetOrders();
        return new List<Order>();
    }
}
