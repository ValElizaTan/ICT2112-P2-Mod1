using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Data.Module1.Interfaces
{
    public interface IOrderStatusHistory
    {
        List<OrderStatusHistory> FindByOrderItem(int orderItemId);
        void InsertStatusHistory(OrderStatusHistory history);
    }
}