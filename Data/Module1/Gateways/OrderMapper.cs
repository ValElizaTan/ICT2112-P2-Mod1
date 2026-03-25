using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderMapper : IOrderMapper
    {
        private readonly AppDbContext _context;

        public OrderMapper(AppDbContext context)
        {
            _context = context;
        }

        public Order? FindOrderById(int orderId)
        {
            return _context.Orders
                .AsEnumerable()
                .FirstOrDefault(o => o.GetOrderId() == orderId);
        }

        public List<Order> FindOrdersByCustomerId(int customerId)
        {
            return _context.Orders
                .AsEnumerable()
                .Where(o => o.GetCustomerId() == customerId)
                .OrderByDescending(o => o.GetOrderDate())
                .ToList();
        }

        public List<Order> FindOrdersByStatus(OrderStatus status)
        {
            var latestOrderIds = _context.Orderstatushistories
                .AsEnumerable()
                .GroupBy(h => h.GetOrderId())
                .Select(g => g.OrderByDescending(x => x.GetTimestamp()).FirstOrDefault())
                .Where(h => h != null && h.GetStatus() == (OrderHistoryStatus)(int)status)
                .Select(h => h!.GetOrderId())
                .ToList();

            return _context.Orders
                .AsEnumerable()
                .Where(o => latestOrderIds.Contains(o.GetOrderId()))
                .OrderByDescending(o => o.GetOrderDate())
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var order = _context.Orders
                .AsEnumerable()
                .FirstOrDefault(o => o.GetOrderId() == orderId);

            if (order == null)
            {
                return;
            }

            order.SetStatus(status);
            _context.SaveChanges();
        }

        public List<Order> FindAllOrders()
        {
            return _context.Orders
                .AsEnumerable()
                .OrderByDescending(o => o.GetOrderDate())
                .ToList();
        }
    }
}