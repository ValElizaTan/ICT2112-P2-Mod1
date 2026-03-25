using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Interfaces;

// Interface contract for Team 6's order service
public interface IOrderService
{
    Order CreateOrder(int customerId, List<Orderitem> items, DeliveryType deliveryMethod);
    OrderStatus GetOrderStatus(int orderId);
    List<Order> GetCustomerOrders(int customerId);
    bool CancelOrder(int orderId, int customerId);
}
