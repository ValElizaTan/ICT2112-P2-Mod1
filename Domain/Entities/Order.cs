using System;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities
{
    public partial class Order
    {
        public int GetOrderId() => _orderid;
        public int GetCustomerId() => _customerid;
        public DateTime GetOrderDate() => _orderdate;
        public decimal GetTotalAmount() => _totalamount;

        public void SetOrderId(int orderId) => _orderid = orderId;
        public void SetCustomerId(int customerId) => _customerid = customerId;
        public void SetOrderDate(DateTime orderDate) => _orderdate = orderDate;
        public void SetTotalAmount(decimal totalAmount) => _totalamount = totalAmount;
        public OrderStatus? GetStatus() => _status;
        public void SetStatus(OrderStatus status)
        {
            _status = status;
        }
    }
}