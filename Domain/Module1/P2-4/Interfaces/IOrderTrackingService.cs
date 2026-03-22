using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Interfaces
{
    public interface IOrderTrackingService
    {
        OrderStatus GetCurrentOrderStatus(int orderId);
        OrderStatus GetCurrentItemStatus(int orderItemId);
        List<OrderStatusHistory> GetTimeline(int orderItemId);
        void UpdateStatus(int orderItemId, OrderStatus newStatus, string remark, int staffId);
        List<string> BulkUpdateStatus(List<int> orderItemIds, OrderStatus newStatus, string remark, int staffId);
        List<Orderitem> FilterItemsByStatus(OrderStatus status);
        bool ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus);
        bool IsFinalStatus(OrderStatus status);
        bool IsRemarkRequired(OrderStatus newStatus);
        bool HasUpdatePermission(int staffId);

        List<Orderitem> GetItemsByOrder(int orderId);
        Order? GetOrderById(int orderId);
        Customer? GetCustomerById(int customerId);
        Staff? GetStaffById(int staffId);
        List<Order> GetOrdersByCustomer(int customerId);
        Orderitem? GetOrderItemById(int orderItemId);
    }
}