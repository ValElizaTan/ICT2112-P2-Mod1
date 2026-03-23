using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities
{
    public partial class Orderitem
    {
        // Added in partial because scaffold Orderitem does not currently include currentStatus
        private OrderStatus _currentStatus;
        public OrderStatus Currentstatus
            {
                get => _currentStatus;
                set => _currentStatus = value;
            }

        public Orderitem(
            int orderItemId,
            int orderId,
            int productId,
            string productName,
            int quantity,
            decimal unitPrice,
            DateTime? rentalStartDate,
            DateTime? rentalEndDate,
            OrderStatus currentStatus)
        {
            _orderitemid = orderItemId;
            _orderid = orderId;
            _productid = productId;
            _productname = productName;
            _quantity = quantity;
            _unitprice = unitPrice;
            _rentalstartdate = rentalStartDate;
            _rentalenddate = rentalEndDate;
            _currentStatus = currentStatus;
        }

        public Orderitem() { }

        public int GetOrderItemId() => _orderitemid;
        public int GetProductId() => _productid;

        public int GetQuantity() => _quantity;
        public void SetQuantity(int quantity) => _quantity = quantity;

        public decimal GetUnitPrice() => _unitprice;

        public DateTime? GetRentalStartDate() => _rentalstartdate;
        public DateTime? GetRentalEndDate() => _rentalenddate;

        public void SetRentalDates(DateTime start, DateTime end)
        {
            _rentalstartdate = start;
            _rentalenddate = end;
        }

        public int GetOrderId() => _orderid;
        public string GetProductName() => _productname;
        public OrderStatus GetCurrentStatus() => _currentStatus;
        public void SetCurrentStatus(OrderStatus status) => _currentStatus = status;
    }
}