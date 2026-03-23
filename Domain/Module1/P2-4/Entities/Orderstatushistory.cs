using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities
{
    public partial class Orderstatushistory
    {
        // Added in partial because scaffold Orderstatushistory
        // currently has no status field and stores updatedBy as string.
        private OrderStatus _status;
        public OrderStatus Status
        {
            get => _status;
            set => _status = value;
        }
        private int _updatedByStaffId;

        public Orderstatushistory(
            int orderItemId,
            OrderStatus status,
            DateTime timestamp,
            int updatedBy,
            string? remark)
        {
            _orderitemid = orderItemId;
            _status = status;
            _timestamp = timestamp;
            _updatedByStaffId = updatedBy;
            _updatedby = updatedBy.ToString();
            _remark = remark;
        }

        protected Orderstatushistory() { }

        public int GetHistoryId() => _historyid;
        public int GetOrderItemId() => _orderitemid;
        public OrderStatus GetStatus() => _status;
        public DateTime GetTimestamp() => _timestamp;
        public int GetUpdatedBy() => _updatedByStaffId;
        public string GetRemark() => _remark ?? string.Empty;

        public void SetStatus(OrderStatus status) => _status = status;
        public void SetTimestamp(DateTime timestamp) => _timestamp = timestamp;

        public void SetUpdatedBy(int staffId)
        {
            _updatedByStaffId = staffId;
            _updatedby = staffId.ToString();
        }

        public void SetRemark(string remark) => _remark = remark;
    }
}