using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls
{
    public class OrderTrackingControl : IOrderTrackingService
    {
        private readonly IOrderMapper _orderMapper;
        private readonly IOrderStatusHistory _orderStatusHistoryGateway;
        private readonly INotificationSubject _notificationSubject;

        public OrderTrackingControl(IOrderMapper orderMapper, IOrderStatusHistory orderStatusHistoryGateway, INotificationSubject notificationSubject)
        {
            _orderMapper = orderMapper;
            _orderStatusHistoryGateway = orderStatusHistoryGateway;
            _notificationSubject = notificationSubject;
        }

        public OrderStatus GetCurrentOrderStatus(int orderId)
        {
            var latest = _orderStatusHistoryGateway.GetLatestByOrderId(orderId);
            if (latest == null)
            {
                return OrderStatus.PENDING;
            }

            return latest.GetStatus();
        }

        public List<Orderstatushistory> GetOrderTimeline(int orderId)
        {
            return _orderStatusHistoryGateway.GetTimelineByOrderId(orderId);
        }

        public void UpdateStatus(int orderId, OrderStatus newStatus, string? remark, int staffId)
        {
            var order = _orderMapper.FindOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            if (!HasUpdatePermission(staffId))
            {
                throw new InvalidOperationException("Staff does not have permission to update order status.");
            }

            var currentStatus = GetCurrentOrderStatus(orderId);

            if (IsFinalStatus(currentStatus))
            {
                throw new InvalidOperationException($"Cannot update order {orderId} because it is already in final status {currentStatus}.");
            }

            if (!ValidateStatusTransition(currentStatus, newStatus))
            {
                throw new InvalidOperationException($"Invalid status transition from {currentStatus} to {newStatus}.");
            }

            if (IsRemarkRequired(newStatus) && string.IsNullOrWhiteSpace(remark))
            {
                throw new InvalidOperationException($"Remark is required for status {newStatus}.");
            }

            _orderMapper.UpdateOrderStatus(orderId, newStatus);

            var history = new Orderstatushistory(
                historyId: 0,
                orderId: orderId,
                status: newStatus,
                timestamp: DateTime.UtcNow,
                updatedBy: staffId.ToString(),
                remark: string.IsNullOrWhiteSpace(remark) ? null : remark
            );

            _orderStatusHistoryGateway.InsertHistory(history);

            // Notify customer about order status change
            var customerId = order.GetCustomerId();
            var notificationMessage = $"Your order #{orderId} status has been updated to {newStatus}.";
            _notificationSubject.CreateNotification(customerId, notificationMessage, NotificationType.ORDER_UPDATE);
        }

        public List<string> BulkUpdateStatus(List<int> orderIds, OrderStatus newStatus, string? remark, int staffId)
        {
            var messages = new List<string>();

            foreach (var orderId in orderIds)
            {
                try
                {
                    UpdateStatus(orderId, newStatus, remark, staffId);
                    messages.Add($"Order {orderId}: updated successfully.");
                }
                catch (Exception ex)
                {
                    messages.Add($"Order {orderId}: {ex.Message}");
                }
            }

            return messages;
        }

        public List<Order> FilterOrdersByStatus(OrderStatus status)
        {
            return _orderMapper.FindOrdersByStatus(status);
        }

        public Order? SearchOrderById(int orderId)
        {
            return _orderMapper.FindOrderById(orderId);
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orderMapper.FindOrdersByCustomerId(customerId);
        }

        public bool ValidateStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            if (currentStatus == newStatus)
            {
                return false;
            }

            return currentStatus switch
            {
                OrderStatus.PENDING => newStatus == OrderStatus.CONFIRMED || newStatus == OrderStatus.CANCELLED,
                OrderStatus.CONFIRMED => newStatus == OrderStatus.PROCESSING || newStatus == OrderStatus.CANCELLED,
                OrderStatus.PROCESSING => newStatus == OrderStatus.READY_FOR_DISPATCH || newStatus == OrderStatus.CANCELLED,
                OrderStatus.READY_FOR_DISPATCH => newStatus == OrderStatus.DISPATCHED || newStatus == OrderStatus.CANCELLED,
                OrderStatus.DISPATCHED => newStatus == OrderStatus.DELIVERED,
                OrderStatus.DELIVERED => false,
                OrderStatus.CANCELLED => false,
                _ => false
            };
        }

        public bool IsFinalStatus(OrderStatus status)
        {
            return status == OrderStatus.CANCELLED || status == OrderStatus.DELIVERED;
        }

        public bool IsRemarkRequired(OrderStatus newStatus)
        {
            return newStatus == OrderStatus.CANCELLED;
        }

        public bool HasUpdatePermission(int staffId)
        {
            return staffId > 0;
        }

        public List<Order> GetAllOrders()
        {
            return _orderMapper.FindAllOrders();
        }
    }
}