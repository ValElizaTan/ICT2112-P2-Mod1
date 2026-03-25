using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderStatusHistoryGateway : IOrderStatusHistory
    {
        private readonly AppDbContext _context;

        public OrderStatusHistoryGateway(AppDbContext context)
        {
            _context = context;
        }

        public List<Orderstatushistory> GetTimelineByOrderId(int orderId)
        {
            return _context.Orderstatushistories
                .AsEnumerable()
                .Where(h => h.GetOrderId() == orderId)
                .OrderByDescending(h => h.GetTimestamp())
                .ToList();
        }

        public Orderstatushistory? GetLatestByOrderId(int orderId)
        {
            return _context.Orderstatushistories
                .AsEnumerable()
                .Where(h => h.GetOrderId() == orderId)
                .OrderByDescending(h => h.GetTimestamp())
                .FirstOrDefault();
        }

        public void InsertHistory(Orderstatushistory history)
        {
            _context.Orderstatushistories.Add(history);
            _context.SaveChanges();
        }

        public List<Orderstatushistory> GetHistoryByStatus(OrderHistoryStatus status)
        {
            return _context.Orderstatushistories
                .AsEnumerable()
                .Where(h => h.GetStatus() == status)
                .OrderByDescending(h => h.GetTimestamp())
                .ToList();
        }
    }
}