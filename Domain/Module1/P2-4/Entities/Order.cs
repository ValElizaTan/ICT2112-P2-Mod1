using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities
{
    public partial class Order
    {
        public Order(int orderId, int customerId, DateTime orderDate, decimal totalAmount, OrderStatus status)
        {
            _orderid = orderId;
            _customerid = customerId;
            _orderdate = orderDate;
            _totalamount = totalAmount;
            _status = status;
        }

        protected Order() { }

        public int GetOrderId() => _orderid;
        public void SetOrderId(int orderId) => _orderid = orderId;

        public int GetCustomerId() => _customerid;
        public void SetCustomerId(int customerId) => _customerid = customerId;

        public DateTime GetOrderDate() => _orderdate;
        public void SetOrderDate(DateTime orderDate) => _orderdate = orderDate;

        public OrderStatus GetStatus() => _status ?? OrderStatus.PENDING;
        public void SetStatus(OrderStatus status) => _status = status;

        public decimal GetTotalAmount() => _totalamount;
        public void SetTotalAmount(decimal totalAmount) => _totalamount = totalAmount;

        public void AttachOrderItems(IEnumerable<Orderitem> items)
        {
            Orderitems.Clear();
            foreach (var item in items)
            {
                Orderitems.Add(item);
            }
        }
    }
}