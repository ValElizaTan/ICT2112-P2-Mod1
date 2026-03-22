using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Enums;

namespace ProRental.Data.Module1.Gateways
{
    public class OrderStatusHistoryGateway : IOrderStatusHistory
    {
        // --------------------------------------------------------------------
        // HARDCODED TEMP DATA
        // Use this first because DB is not working at the moment.
        // Replace with actual DB code later.
        // --------------------------------------------------------------------
        private static readonly List<OrderStatusHistory> _history = new()
        {
            new OrderStatusHistory
            {
                HistoryId = 1,
                OrderItemId = 5001,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-10).AddHours(9),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 2,
                OrderItemId = 5001,
                Status = OrderStatus.PACKING,
                Timestamp = DateTime.Today.AddDays(-9).AddHours(14),
                UpdatedBy = 101,
                Remark = "Packing camera body."
            },
            new OrderStatusHistory
            {
                HistoryId = 3,
                OrderItemId = 5001,
                Status = OrderStatus.DISPATCHED,
                Timestamp = DateTime.Today.AddDays(-8).AddHours(10),
                UpdatedBy = 102,
                Remark = "Dispatched to customer."
            },
            new OrderStatusHistory
            {
                HistoryId = 4,
                OrderItemId = 5001,
                Status = OrderStatus.DELIVERED,
                Timestamp = DateTime.Today.AddDays(-7).AddHours(13),
                UpdatedBy = 102,
                Remark = "Delivered successfully."
            },
            new OrderStatusHistory
            {
                HistoryId = 5,
                OrderItemId = 5001,
                Status = OrderStatus.IN_RENTAL,
                Timestamp = DateTime.Today.AddDays(-2).AddHours(9),
                UpdatedBy = 101,
                Remark = "Rental period started."
            },

            new OrderStatusHistory
            {
                HistoryId = 6,
                OrderItemId = 5002,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-10).AddHours(9),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 7,
                OrderItemId = 5002,
                Status = OrderStatus.PACKING,
                Timestamp = DateTime.Today.AddDays(-9).AddHours(14),
                UpdatedBy = 101,
                Remark = "Packing lens."
            },
            new OrderStatusHistory
            {
                HistoryId = 8,
                OrderItemId = 5002,
                Status = OrderStatus.DISPATCHED,
                Timestamp = DateTime.Today.AddDays(-8).AddHours(10),
                UpdatedBy = 102,
                Remark = "Dispatched together with camera."
            },
            new OrderStatusHistory
            {
                HistoryId = 9,
                OrderItemId = 5002,
                Status = OrderStatus.DELIVERED,
                Timestamp = DateTime.Today.AddDays(-7).AddHours(13),
                UpdatedBy = 102,
                Remark = "Delivered successfully."
            },
            new OrderStatusHistory
            {
                HistoryId = 10,
                OrderItemId = 5002,
                Status = OrderStatus.IN_RENTAL,
                Timestamp = DateTime.Today.AddDays(-2).AddHours(9),
                UpdatedBy = 101,
                Remark = "Rental period started."
            },

            new OrderStatusHistory
            {
                HistoryId = 11,
                OrderItemId = 5003,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-4).AddHours(11),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 12,
                OrderItemId = 5003,
                Status = OrderStatus.PACKING,
                Timestamp = DateTime.Today.AddDays(-3).AddHours(16),
                UpdatedBy = 101,
                Remark = "Preparing camera body."
            },
            new OrderStatusHistory
            {
                HistoryId = 13,
                OrderItemId = 5003,
                Status = OrderStatus.DISPATCHED,
                Timestamp = DateTime.Today.AddDays(-2).AddHours(15),
                UpdatedBy = 102,
                Remark = "Out for delivery."
            },
            new OrderStatusHistory
            {
                HistoryId = 14,
                OrderItemId = 5003,
                Status = OrderStatus.DELIVERED,
                Timestamp = DateTime.Today.AddDays(-1).AddHours(17),
                UpdatedBy = 102,
                Remark = "Received by customer."
            },

            new OrderStatusHistory
            {
                HistoryId = 15,
                OrderItemId = 5004,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-4).AddHours(11),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 16,
                OrderItemId = 5004,
                Status = OrderStatus.PACKING,
                Timestamp = DateTime.Today.AddDays(-1).AddHours(10),
                UpdatedBy = 101,
                Remark = "Packing tripod stand."
            },

            new OrderStatusHistory
            {
                HistoryId = 17,
                OrderItemId = 5005,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-15).AddHours(8),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 18,
                OrderItemId = 5005,
                Status = OrderStatus.PACKING,
                Timestamp = DateTime.Today.AddDays(-14).AddHours(10),
                UpdatedBy = 101,
                Remark = "Packed and checked."
            },
            new OrderStatusHistory
            {
                HistoryId = 19,
                OrderItemId = 5005,
                Status = OrderStatus.DISPATCHED,
                Timestamp = DateTime.Today.AddDays(-13).AddHours(9),
                UpdatedBy = 102,
                Remark = "Dispatched."
            },
            new OrderStatusHistory
            {
                HistoryId = 20,
                OrderItemId = 5005,
                Status = OrderStatus.DELIVERED,
                Timestamp = DateTime.Today.AddDays(-12).AddHours(14),
                UpdatedBy = 102,
                Remark = "Delivered."
            },
            new OrderStatusHistory
            {
                HistoryId = 21,
                OrderItemId = 5005,
                Status = OrderStatus.IN_RENTAL,
                Timestamp = DateTime.Today.AddDays(-12).AddHours(18),
                UpdatedBy = 101,
                Remark = "Rental started."
            },
            new OrderStatusHistory
            {
                HistoryId = 22,
                OrderItemId = 5005,
                Status = OrderStatus.RETURN_PICKUP,
                Timestamp = DateTime.Today.AddDays(-9).AddHours(10),
                UpdatedBy = 102,
                Remark = "Pickup scheduled."
            },
            new OrderStatusHistory
            {
                HistoryId = 23,
                OrderItemId = 5005,
                Status = OrderStatus.RETURNED,
                Timestamp = DateTime.Today.AddDays(-8).AddHours(15),
                UpdatedBy = 102,
                Remark = "Item returned."
            },
            new OrderStatusHistory
            {
                HistoryId = 24,
                OrderItemId = 5005,
                Status = OrderStatus.INSPECTION,
                Timestamp = DateTime.Today.AddDays(-8).AddHours(17),
                UpdatedBy = 101,
                Remark = "No visible damage."
            },
            new OrderStatusHistory
            {
                HistoryId = 25,
                OrderItemId = 5005,
                Status = OrderStatus.COMPLETED,
                Timestamp = DateTime.Today.AddDays(-7).AddHours(9),
                UpdatedBy = 102,
                Remark = "Order item completed."
            },

            new OrderStatusHistory
            {
                HistoryId = 26,
                OrderItemId = 5006,
                Status = OrderStatus.PLACED,
                Timestamp = DateTime.Today.AddDays(-15).AddHours(8),
                UpdatedBy = 101,
                Remark = "Order created."
            },
            new OrderStatusHistory
            {
                HistoryId = 27,
                OrderItemId = 5006,
                Status = OrderStatus.CANCELLED,
                Timestamp = DateTime.Today.AddDays(-14).AddHours(11),
                UpdatedBy = 102,
                Remark = "Customer requested cancellation due to schedule change."
            }
        };

        public List<OrderStatusHistory> FindByOrderItem(int orderItemId)
        {
            return _history
                .Where(x => x.OrderItemId == orderItemId)
                .OrderBy(x => x.Timestamp)
                .ToList();

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            return db.OrderStatusHistory
                     .Where(x => x.OrderItemId == orderItemId)
                     .OrderBy(x => x.Timestamp)
                     .ToList();
            */
        }

        public void InsertStatusHistory(OrderStatusHistory history)
        {
            history.HistoryId = _history.Any() ? _history.Max(x => x.HistoryId) + 1 : 1;
            _history.Add(history);

            /*
            // DB CODE - COMMENTED OUT FOR NOW
            using var db = new AppDbContext(...);
            db.OrderStatusHistory.Add(history);
            db.SaveChanges();
            */
        }
    }
}