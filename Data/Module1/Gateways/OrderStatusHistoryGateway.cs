using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Data.UnitOfWork;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderStatusHistoryGateway : IOrderStatusHistory
    {
        private readonly AppDbContext _dbContext;

        public OrderStatusHistoryGateway(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Orderstatushistory> FindByOrderItem(int orderItemId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.StatusHistories
            //     .Where(h => h.GetOrderItemId() == orderItemId)
            //     .OrderBy(h => h.GetTimestamp())
            //     .ToList();

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Orderstatushistories
                .ToList()
                .Where(h => h.GetOrderItemId() == orderItemId)
                .OrderBy(h => h.GetTimestamp())
                .ToList();
        }

        public void InsertStatusHistory(Orderstatushistory history)
        {
            // HARD-CODED DEMO DATA
            // DemoOrderTrackingStore.AddHistory(history);

            // REPLACE THIS WHEN DATABASE IS READY
            _dbContext.Orderstatushistories.Add(history);
            _dbContext.SaveChanges();
        }
    }
}