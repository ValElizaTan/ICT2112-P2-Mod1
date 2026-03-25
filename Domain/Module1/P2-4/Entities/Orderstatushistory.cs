using System;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities
{
    public partial class Orderstatushistory
    {
        private OrderHistoryStatus _status;
        private OrderHistoryStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public Orderstatushistory(int historyId, int orderId, OrderHistoryStatus status, DateTime timestamp, string updatedBy, string? remark)
        {
            _historyid = historyId;
            _orderid = orderId;
            _status = status;
            _timestamp = timestamp;
            _updatedby = updatedBy;
            _remark = remark;
        }

        protected Orderstatushistory() { }

        public int GetHistoryId() => _historyid;
        public void SetHistoryId(int historyId) => _historyid = historyId;

        public int GetOrderId() => _orderid;
        public void SetOrderId(int orderId) => _orderid = orderId;

        public OrderHistoryStatus GetStatus() => _status;
        public void SetStatus(OrderHistoryStatus status) => _status = status;

        public DateTime GetTimestamp() => _timestamp;
        public void SetTimestamp(DateTime timestamp) => _timestamp = timestamp;

        public string GetUpdatedBy() => _updatedby;
        public void SetUpdatedBy(string updatedBy) => _updatedby = updatedBy;

        public string? GetRemark() => _remark;
        public void SetRemark(string? remark) => _remark = remark;
    }
}