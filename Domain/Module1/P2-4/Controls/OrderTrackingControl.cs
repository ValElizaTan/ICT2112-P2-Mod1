using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls
{
    public class OrderTrackingControl : IOrderTrackingService
    {
        private readonly IOrderMapper _orderMapper;
        private readonly IOrderStatusHistory _statusHistoryGateway;

        public OrderTrackingControl(IOrderMapper orderMapper, IOrderStatusHistory statusHistoryGateway)
        {
            _orderMapper = orderMapper;
            _statusHistoryGateway = statusHistoryGateway;
        }

        public OrderStatus GetCurrentOrderStatus(int orderId)
        {
            var items = _orderMapper.FindOrderItemsByOrder(orderId);
            if (!items.Any())
            {
                throw new ArgumentException("Order not found or contains no order items.");
            }

            // Use the latest timestamp across all item histories as the order's current status.
            // This works better for mixed item statuses in a real rental order.
            var latestHistory = items
                .SelectMany(item => _statusHistoryGateway.FindByOrderItem(item.OrderItemId))
                .OrderByDescending(h => h.Timestamp)
                .FirstOrDefault();

            return latestHistory?.Status ?? items.First().CurrentStatus;
        }

        public OrderStatus GetCurrentItemStatus(int orderItemId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException("Invalid order item ID.");
            }

            return item.CurrentStatus;
        }

        public List<OrderStatusHistory> GetTimeline(int orderItemId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException("Invalid order item ID.");
            }

            return _statusHistoryGateway.FindByOrderItem(orderItemId);
        }

        public void UpdateStatus(int orderItemId, OrderStatus newStatus, string remark, int staffId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException($"Order item {orderItemId} was not found.");
            }

            if (!HasUpdatePermission(staffId))
            {
                throw new InvalidOperationException("Staff does not have permission to update status.");
            }

            var currentStatus = item.CurrentStatus;

            if (IsFinalStatus(currentStatus))
            {
                throw new InvalidOperationException($"Order item {orderItemId} is already in final status {currentStatus} and cannot be updated.");
            }

            if (!ValidateStatusTransition(currentStatus, newStatus))
            {
                throw new InvalidOperationException($"Invalid status transition from {currentStatus} to {newStatus}.");
            }

            if (IsRemarkRequired(newStatus) && string.IsNullOrWhiteSpace(remark))
            {
                throw new InvalidOperationException($"Remark is required when updating to {newStatus}.");
            }

            _orderMapper.UpdateCurrentStatus(orderItemId, newStatus);

            var history = new OrderStatusHistory
            {
                OrderItemId = orderItemId,
                Status = newStatus,
                Timestamp = DateTime.Now,
                UpdatedBy = staffId,
                Remark = remark?.Trim() ?? string.Empty
            };

            _statusHistoryGateway.InsertStatusHistory(history);
        }

        public List<string> BulkUpdateStatus(List<int> orderItemIds, OrderStatus newStatus, string remark, int staffId)
        {
            var results = new List<string>();

            foreach (var orderItemId in orderItemIds.Distinct())
            {
                try
                {
                    UpdateStatus(orderItemId, newStatus, remark, staffId);
                    results.Add($"Order item {orderItemId}: updated successfully.");
                }
                catch (Exception ex)
                {
                    results.Add($"Order item {orderItemId}: failed - {ex.Message}");
                }
            }

            return results;
        }

        public List<Orderitem> FilterItemsByStatus(OrderStatus status)
        {
            return _orderMapper.FindOrderItemsByStatus(status);
        }

        public bool ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                return false;
            }

            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                { OrderStatus.PLACED, new List<OrderStatus> { OrderStatus.PACKING, OrderStatus.CANCELLED } },
                { OrderStatus.PACKING, new List<OrderStatus> { OrderStatus.DISPATCHED, OrderStatus.CANCELLED } },
                { OrderStatus.DISPATCHED, new List<OrderStatus> { OrderStatus.DELIVERED, OrderStatus.CANCELLED } },
                { OrderStatus.DELIVERED, new List<OrderStatus> { OrderStatus.IN_RENTAL, OrderStatus.RETURN_PICKUP } },
                { OrderStatus.IN_RENTAL, new List<OrderStatus> { OrderStatus.RETURN_PICKUP, OrderStatus.CANCELLED } },
                { OrderStatus.RETURN_PICKUP, new List<OrderStatus> { OrderStatus.RETURNED } },
                { OrderStatus.RETURNED, new List<OrderStatus> { OrderStatus.INSPECTION } },
                { OrderStatus.INSPECTION, new List<OrderStatus> { OrderStatus.REFUND_PROCESSING, OrderStatus.COMPLETED } },
                { OrderStatus.REFUND_PROCESSING, new List<OrderStatus> { OrderStatus.COMPLETED } },
                { OrderStatus.CANCELLED, new List<OrderStatus>() },
                { OrderStatus.COMPLETED, new List<OrderStatus>() }
            };

            return allowedTransitions.ContainsKey(currentStatus)
                   && allowedTransitions[currentStatus].Contains(newStatus);
        }

        public bool IsFinalStatus(OrderStatus status)
        {
            return status == OrderStatus.CANCELLED || status == OrderStatus.COMPLETED;
        }

        public bool IsRemarkRequired(OrderStatus newStatus)
        {
            return newStatus == OrderStatus.CANCELLED
                   || newStatus == OrderStatus.REFUND_PROCESSING
                   || newStatus == OrderStatus.RETURN_PICKUP
                   || newStatus == OrderStatus.INSPECTION;
        }

        public bool HasUpdatePermission(int staffId)
        {
            var staff = _orderMapper.FindStaffById(staffId);
            return staff != null;
        }

        public List<Orderitem> GetItemsByOrder(int orderId)
        {
            return _orderMapper.FindOrderItemsByOrder(orderId);
        }

        public Order? GetOrderById(int orderId)
        {
            return _orderMapper.FindOrderById(orderId);
        }

        public Customer? GetCustomerById(int customerId)
        {
            return _orderMapper.FindCustomerById(customerId);
        }

        public Staff? GetStaffById(int staffId)
        {
            return _orderMapper.FindStaffById(staffId);
        }

        public List<Order> GetOrdersByCustomer(int customerId)
        {
            return _orderMapper.FindOrdersByCustomer(customerId);
        }

        public Orderitem? GetOrderItemById(int orderItemId)
        {
            return _orderMapper.FindOrderItemById(orderItemId);
        }
    }
}