using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Interfaces
{
    public interface IOrderTrackingService
    {
        OrderStatus GetCurrentOrderStatus(int orderId);
        List<Orderstatushistory> GetOrderTimeline(int orderId);
        void UpdateStatus(int orderId, OrderStatus newStatus, string? remark, int staffId);
        List<string> BulkUpdateStatus(List<int> orderIds, OrderStatus newStatus, string? remark, int staffId);
        List<Order> FilterOrdersByStatus(OrderStatus status);
        Order? SearchOrderById(int orderId);
        bool ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus);
        bool IsFinalStatus(OrderStatus status);
        bool IsRemarkRequired(OrderStatus newStatus);
        bool HasUpdatePermission(int staffId);
        List<Order> GetOrdersByCustomerId(int customerId);
        List<Order> GetAllOrders();
    }
}