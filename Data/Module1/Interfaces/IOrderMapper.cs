using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Interfaces
{
    public interface IOrderMapper
    {
        List<Orderitem> FindOrderItemsByOrder(int orderId);
        Orderitem? FindOrderItemById(int orderItemId);
        List<Orderitem> FindOrderItemsByStatus(OrderStatus status);
        void UpdateCurrentStatus(int orderItemId, OrderStatus status);

        // Minimal compile-ready additions required by Order Tracking
        Order? FindOrderById(int orderId);
        List<Order> FindOrdersByCustomer(int customerId);
        Customer? FindCustomerById(int customerId);
        Staff? FindStaffById(int staffId);
    }
}