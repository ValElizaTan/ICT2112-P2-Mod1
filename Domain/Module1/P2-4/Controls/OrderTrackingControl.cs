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
            var order = _orderMapper.FindOrderById(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order ID {orderId} not found.");
            }

            return order.GetStatus();
        }

        public OrderStatus GetCurrentItemStatus(int orderItemId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException($"Order item ID {orderItemId} not found.");
            }

            return item.GetCurrentStatus();
        }

        public List<Orderstatushistory> GetTimeline(int orderItemId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException($"Order item ID {orderItemId} not found.");
            }

            return _statusHistoryGateway.FindByOrderItem(orderItemId);
        }

        public void UpdateStatus(int orderItemId, OrderStatus newStatus, string remark, int staffId)
        {
            var item = _orderMapper.FindOrderItemById(orderItemId);
            if (item == null)
            {
                throw new ArgumentException($"Order item ID {orderItemId} not found.");
            }

            if (!HasUpdatePermission(staffId))
            {
                throw new UnauthorizedAccessException($"Staff ID {staffId} has no update permission.");
            }

            var currentStatus = item.GetCurrentStatus();

            if (IsFinalStatus(currentStatus))
            {
                throw new InvalidOperationException($"Order item {orderItemId} is already in final status {currentStatus}.");
            }

            if (IsRemarkRequired(newStatus) && string.IsNullOrWhiteSpace(remark))
            {
                throw new InvalidOperationException($"Remark is required when updating to {newStatus}.");
            }

            if (!ValidateStatusTransition(currentStatus, newStatus))
            {
                throw new InvalidOperationException($"Invalid status transition from {currentStatus} to {newStatus}.");
            }

            _orderMapper.UpdateCurrentStatus(orderItemId, newStatus);

            var existingHistory = _statusHistoryGateway.FindByOrderItem(orderItemId) ?? new List<Orderstatushistory>();
            var nextHistoryId = existingHistory.Any() ? existingHistory.Max(h => h.GetHistoryId()) + 1 : 1;

            var history = new Orderstatushistory(
                orderItemId: orderItemId,
                status: newStatus,
                timestamp: DateTime.UtcNow,
                updatedBy: staffId,
                remark: remark ?? string.Empty
            );

            _statusHistoryGateway.InsertStatusHistory(history);

            SyncOrderStatusFromItems(item.GetOrderId());
        }

        public List<string> BulkUpdateStatus(List<int> orderItemIds, OrderStatus newStatus, string remark, int staffId)
        {
            var results = new List<string>();

            if (orderItemIds == null || orderItemIds.Count == 0)
            {
                results.Add("No order item IDs provided.");
                return results;
            }

            foreach (var orderItemId in orderItemIds)
            {
                try
                {
                    UpdateStatus(orderItemId, newStatus, remark, staffId);
                    results.Add($"Order item {orderItemId}: update successful.");
                }
                catch (Exception ex)
                {
                    results.Add($"Order item {orderItemId}: {ex.Message}");
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

            var transitions = new Dictionary<OrderStatus, List<OrderStatus>>
            {
                { OrderStatus.PENDING, new List<OrderStatus> { OrderStatus.CONFIRMED, OrderStatus.CANCELLED } },
                { OrderStatus.CONFIRMED, new List<OrderStatus> { OrderStatus.PACKING, OrderStatus.CANCELLED } },
                { OrderStatus.PACKING, new List<OrderStatus> { OrderStatus.READY_FOR_DISPATCH, OrderStatus.CANCELLED } },
                { OrderStatus.READY_FOR_DISPATCH, new List<OrderStatus> { OrderStatus.DISPATCHED, OrderStatus.CANCELLED } },
                { OrderStatus.DISPATCHED, new List<OrderStatus> { OrderStatus.DELIVERED } },
                { OrderStatus.DELIVERED, new List<OrderStatus> { OrderStatus.IN_RENTAL } },
                { OrderStatus.IN_RENTAL, new List<OrderStatus> { OrderStatus.RETURN_PICKUP } },
                { OrderStatus.RETURN_PICKUP, new List<OrderStatus> { OrderStatus.RETURNED } },
                { OrderStatus.RETURNED, new List<OrderStatus> { OrderStatus.INSPECTION } },
                { OrderStatus.INSPECTION, new List<OrderStatus> { OrderStatus.REFUND_PROCESSING, OrderStatus.COMPLETED } },
                { OrderStatus.REFUND_PROCESSING, new List<OrderStatus> { OrderStatus.COMPLETED } },
                { OrderStatus.CANCELLED, new List<OrderStatus>() },
                { OrderStatus.COMPLETED, new List<OrderStatus>() }
            };

            return transitions.TryGetValue(currentStatus, out var allowedStatuses)
                   && allowedStatuses.Contains(newStatus);
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
            // allow all demo staff IDs
            return GetStaffById(staffId) != null;
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

        private void SyncOrderStatusFromItems(int orderId)
        {
            var order = _orderMapper.FindOrderById(orderId);
            if (order == null)
            {
                return;
            }

            var items = _orderMapper.FindOrderItemsByOrder(orderId) ?? new List<Orderitem>();
            if (!items.Any())
            {
                return;
            }

            // Simple sync rule:
            // - if all items same status, order takes that status
            // - if all are final and at least one completed, set COMPLETED
            // - otherwise keep current order status
            if (items.All(i => i != null && i.GetCurrentStatus() == items[0].GetCurrentStatus()))
            {
                order.SetStatus(items[0].GetCurrentStatus());
                return;
            }

            if (items.All(i => i != null && IsFinalStatus(i.GetCurrentStatus())) &&
                items.Any(i => i != null && i.GetCurrentStatus() == OrderStatus.COMPLETED))
            {
                order.SetStatus(OrderStatus.COMPLETED);
            }
        }
    }
}