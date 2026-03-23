using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Data.UnitOfWork;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderMapper : IOrderMapper
    {
        private readonly AppDbContext _dbContext;

        public OrderMapper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Orderitem> FindOrderItemsByOrder(int orderId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Orderitems
            //     .Where(i => i.GetOrderId() == orderId)
            //     .ToList();

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Orderitems
                .ToList()
                .Where(i => i.GetOrderId() == orderId)
                .ToList();
        }

        public Orderitem? FindOrderItemById(int orderItemId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Orderitems
            //     .FirstOrDefault(i => i.GetOrderItemId() == orderItemId);

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Orderitems
                .ToList()
                .FirstOrDefault(i => i.GetOrderItemId() == orderItemId);
        }

        public List<Orderitem> FindOrderItemsByStatus(OrderStatus status)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Orderitems
            //     .Where(i => i.GetCurrentStatus() == status)
            //     .ToList();

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Orderitems
                .ToList()
                .Where(i => i.GetCurrentStatus() == status)
                .ToList();
        }

        public void UpdateCurrentStatus(int orderItemId, OrderStatus status)
        {
            // HARD-CODED DEMO DATA
            // var item = DemoOrderTrackingStore.Orderitems
            //     .FirstOrDefault(i => i.GetOrderItemId() == orderItemId);

            // if (item != null)
            // {
            //     item.SetCurrentStatus(status);
            // }

            // REPLACE THIS WHEN DATABASE IS READY
            var item = _dbContext.Orderitems
                .ToList()
                .FirstOrDefault(i => i.GetOrderItemId() == orderItemId);

            if (item == null)
            {
                throw new ArgumentException($"Order item ID {orderItemId} not found.");
            }

            item.SetCurrentStatus(status);
            _dbContext.SaveChanges();
        }

        public Order? FindOrderById(int orderId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Orders
            //     .FirstOrDefault(o => o.GetOrderId() == orderId);

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Orders
                .ToList()
                .FirstOrDefault(o => o.GetOrderId() == orderId);
        }

        public List<Order> FindOrdersByCustomer(int customerId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Orders
            //     .Where(o => o.GetCustomerId() == customerId)
            //     .OrderByDescending(o => o.GetOrderDate())
            //     .ToList();

            // REPLACE THIS WHEN DATABASE IS READY
            var orders = _dbContext.Orders
                .ToList()
                .Where(o => o.GetCustomerId() == customerId)
                .OrderByDescending(o => o.GetOrderDate())
                .ToList();

            foreach (var order in orders)
            {
                var items = _dbContext.Orderitems
                    .ToList()
                    .Where(i => i.GetOrderId() == order.GetOrderId())
                    .ToList();

                order.AttachOrderItems(items);
            }

            return orders;
        }

        public Customer? FindCustomerById(int customerId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Customers
            //     .FirstOrDefault(c => c.GetCustomerInfo().CustomerId == customerId);

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Customers
                .ToList()
                .FirstOrDefault(c => c.GetCustomerInfo().CustomerId == customerId);
        }

        public Staff? FindStaffById(int staffId)
        {
            // HARD-CODED DEMO DATA
            // return DemoOrderTrackingStore.Staffs
            //     .FirstOrDefault(s => s.GetStaffInfo().StaffId == staffId);

            // REPLACE THIS WHEN DATABASE IS READY
            return _dbContext.Staff
                .ToList()
                .FirstOrDefault(s => s.GetStaffInfo().StaffId == staffId);
        }
    }
}