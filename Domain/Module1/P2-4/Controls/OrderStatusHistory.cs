using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Controls
{
    public class OrderStatusHistory
    {
        public int HistoryId { get; set; }
        public int OrderItemId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public int UpdatedBy { get; set; }
        public string Remark { get; set; } = string.Empty;

        public int GetHistoryId() => HistoryId;
        public int GetOrderItemId() => OrderItemId;
        public OrderStatus GetStatus() => Status;
        public DateTime GetTimestamp() => Timestamp;
        public int GetUpdatedBy() => UpdatedBy;
        public string GetRemark() => Remark;

        public void SetStatus(OrderStatus status) => Status = status;
        public void SetTimestamp(DateTime timestamp) => Timestamp = timestamp;
        public void SetUpdatedBy(int staffId) => UpdatedBy = staffId;
        public void SetRemark(string remark) => Remark = remark;
    }
}