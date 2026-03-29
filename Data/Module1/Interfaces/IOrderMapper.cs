using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Interfaces
{
    public interface IOrderMapper
    {
        Order? FindOrderById(int orderId);
        List<Order> FindOrdersByStatus(OrderStatus status);
        List<Order> FindOrdersByCustomerId(int customerId);
        void UpdateOrderStatus(int orderId, OrderStatus status);
        List<Order> FindAllOrders();
    }
}