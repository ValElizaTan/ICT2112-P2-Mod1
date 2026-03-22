using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderMapper : IOrderMapper
    {
        // --------------------------------------------------------------------
        // HARDCODED TEMP DATA
        // Use this first because DB is not working at the moment.
        // Replace with actual DB code later.
        // --------------------------------------------------------------------
        private static readonly List<Customer> _customers = new()
        {
            new Customer { CustomerId = 1, Name = "Zen Lim" },
            new Customer { CustomerId = 2, Name = "Alicia Tan" }
        };

        private static readonly List<Staff> _staff = new()
        {
            new Staff { StaffId = 101, Name = "Marcus Lee", Role = "Operations Staff" },
            new Staff { StaffId = 102, Name = "Sarah Ong", Role = "Manager" }
        };

        private static readonly List<Order> _orders = new()
        {
            new Order
            {
                OrderId = 1001,
                CustomerId = 1,
                OrderNumber = "PR-2026-1001",
                CreatedAt = DateTime.Today.AddDays(-10)
            },
            new Order
            {
                OrderId = 1002,
                CustomerId = 1,
                OrderNumber = "PR-2026-1002",
                CreatedAt = DateTime.Today.AddDays(-4)
            },
            new Order
            {
                OrderId = 1003,
                CustomerId = 2,
                OrderNumber = "PR-2026-1003",
                CreatedAt = DateTime.Today.AddDays(-15)
            }
        };

        private static readonly List<Orderitem> _orderItems = new()
        {
            new Orderitem
            {
                OrderItemId = 5001,
                OrderId = 1001,
                ProductName = "Canon EOS R6 Camera Body",
                CurrentStatus = OrderStatus.IN_RENTAL,
                Quantity = 1,
                RentalStartDate = DateTime.Today.AddDays(-2),
                RentalEndDate = DateTime.Today.AddDays(3)
            },
            new Orderitem
            {
                OrderItemId = 5002,
                OrderId = 1001,
                ProductName = "RF 24-70mm Lens",
                CurrentStatus = OrderStatus.IN_RENTAL,
                Quantity = 1,
                RentalStartDate = DateTime.Today.AddDays(-2),
                RentalEndDate = DateTime.Today.AddDays(3)
            },
            new Orderitem
            {
                OrderItemId = 5003,
                OrderId = 1002,
                ProductName = "Sony A7 IV Camera Body",
                CurrentStatus = OrderStatus.DELIVERED,
                Quantity = 1,
                RentalStartDate = DateTime.Today,
                RentalEndDate = DateTime.Today.AddDays(5)
            },
            new Orderitem
            {
                OrderItemId = 5004,
                OrderId = 1002,
                ProductName = "Tripod Stand",
                CurrentStatus = OrderStatus.PACKING,
                Quantity = 1,
                RentalStartDate = DateTime.Today,
                RentalEndDate = DateTime.Today.AddDays(5)
            },
            new Orderitem
            {
                OrderItemId = 5005,
                OrderId = 1003,
                ProductName = "DJI RS 3 Gimbal",
                CurrentStatus = OrderStatus.COMPLETED,
                Quantity = 1,
                RentalStartDate = DateTime.Today.AddDays(-12),
                RentalEndDate = DateTime.Today.AddDays(-8)
            },
            new Orderitem
            {
                OrderItemId = 5006,
                OrderId = 1003,
                ProductName = "Lighting Kit",
                CurrentStatus = OrderStatus.CANCELLED,
                Quantity = 2,
                RentalStartDate = DateTime.Today.AddDays(-12),
                RentalEndDate = DateTime.Today.AddDays(-8)
            }
        };

        public List<Orderitem> FindOrderItemsByOrder(int orderId)
        {
            return _orderItems
                .Where(x => x.OrderId == orderId)
                .OrderBy(x => x.OrderItemId)
                .ToList();

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.OrderItems
                     .Where(x => x.OrderId == orderId)
                     .OrderBy(x => x.OrderItemId)
                     .ToList();
            */
        }

        public Orderitem? FindOrderItemById(int orderItemId)
        {
            return _orderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.OrderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
            */
        }

        public List<Orderitem> FindOrderItemsByStatus(OrderStatus status)
        {
            return _orderItems
                .Where(x => x.CurrentStatus == status)
                .OrderBy(x => x.OrderId)
                .ThenBy(x => x.OrderItemId)
                .ToList();

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.OrderItems
                     .Where(x => x.CurrentStatus == status)
                     .OrderBy(x => x.OrderId)
                     .ThenBy(x => x.OrderItemId)
                     .ToList();
            */
        }

        public void UpdateCurrentStatus(int orderItemId, OrderStatus status)
        {
            var item = _orderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
            if (item != null)
            {
                item.CurrentStatus = status;
            }

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            var item = db.OrderItems.FirstOrDefault(x => x.OrderItemId == orderItemId);
            if (item != null)
            {
                item.CurrentStatus = status;
                db.SaveChanges();
            }
            */
        }

        public Order? FindOrderById(int orderId)
        {
            return _orders.FirstOrDefault(x => x.OrderId == orderId);

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.Orders.FirstOrDefault(x => x.OrderId == orderId);
            */
        }

        public Customer? FindCustomerById(int customerId)
        {
            return _customers.FirstOrDefault(x => x.CustomerId == customerId);

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            */
        }

        public Staff? FindStaffById(int staffId)
        {
            return _staff.FirstOrDefault(x => x.StaffId == staffId);

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.Staff.FirstOrDefault(x => x.StaffId == staffId);
            */
        }

        public List<Order> FindOrdersByCustomer(int customerId)
        {
            return _orders
                .Where(x => x.CustomerId == customerId)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.Orders
                     .Where(x => x.CustomerId == customerId)
                     .OrderByDescending(x => x.CreatedAt)
                     .ToList();
            */
        }
    }
}