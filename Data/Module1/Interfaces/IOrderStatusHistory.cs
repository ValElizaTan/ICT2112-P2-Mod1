using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Interfaces
{
    public interface IOrderStatusHistory
    {
        List<Orderstatushistory> GetTimelineByOrderId(int orderId);
        Orderstatushistory? GetLatestByOrderId(int orderId);
        void InsertHistory(Orderstatushistory history);
        List<Orderstatushistory> GetHistoryByStatus(OrderStatus status);
    }
}