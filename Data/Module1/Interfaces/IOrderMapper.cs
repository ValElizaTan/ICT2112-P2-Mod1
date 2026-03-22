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

        Order? FindOrderById(int orderId);
        Customer? FindCustomerById(int customerId);
        Staff? FindStaffById(int staffId);
        List<Order> FindOrdersByCustomer(int customerId);
    }
}