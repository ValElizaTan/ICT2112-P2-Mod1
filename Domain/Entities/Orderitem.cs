using System;
using System.Collections.Generic;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Orderitem
{
    private int _orderitemid;
    private int Orderitemid { get => _orderitemid; set => _orderitemid = value; }

    private int _orderid;
    private int Orderid { get => _orderid; set => _orderid = value; }

    private int _productid;
    private int Productid { get => _productid; set => _productid = value; }

    private int _quantity;
    //private int Quantity { get => _quantity; set => _quantity = value; }

    private decimal _unitprice;
    private decimal Unitprice { get => _unitprice; set => _unitprice = value; }

    private DateTime? _rentalstartdate;
    private DateTime? Rentalstartdate { get => _rentalstartdate; set => _rentalstartdate = value; }

    private DateTime? _rentalenddate;
    private DateTime? Rentalenddate { get => _rentalenddate; set => _rentalenddate = value; }

    public virtual Order Order { get; private set; } = null!;

    public virtual Product Product { get; private set; } = null!;


    // MINIMAL PLACEHOLDER ENTITY
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public OrderStatus CurrentStatus { get; set; }

    // Extra display/testing fields for frontend demo
    public int Quantity { get; set; }
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }

    public int GetOrderItemId() => OrderItemId;
    public int GetOrderId() => OrderId;
    public string GetProductName() => ProductName;
    public OrderStatus GetCurrentStatus() => CurrentStatus;
    public void SetCurrentStatus(OrderStatus status) => CurrentStatus = status;
}
