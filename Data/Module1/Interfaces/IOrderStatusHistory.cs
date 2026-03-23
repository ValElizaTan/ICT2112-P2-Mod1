using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces
{
    public interface IOrderStatusHistory
    {
        List<Orderstatushistory> FindByOrderItem(int orderItemId);
        void InsertStatusHistory(Orderstatushistory history);
    }
}