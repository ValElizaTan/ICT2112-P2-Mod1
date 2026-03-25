using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using System.Reflection;

namespace ProRental.Domain.Module1.P24.Controls;

public class CustomerDashboardControl
{
    private readonly IOrderService? _orderService;
    private readonly ICustomerService? _customerService;
    private readonly IRefundService? _refundService;

    public CustomerDashboardControl(
        IOrderService? orderService = null,
        ICustomerService? customerService = null,
        IRefundService? refundService = null)
    {
        _orderService = orderService;
        _customerService = customerService;
        _refundService = refundService;
    }

    // Helper method to get private field value using reflection
    private T? GetPrivateFieldValue<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null) return default;
        return (T?)field.GetValue(obj);
    }

    // Helper method to get private property value using reflection
    private T? GetPrivatePropertyValue<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (property == null) return default;
        return (T?)property.GetValue(obj);
    }

    public List<Order> GetCustomerOrders(int customerId)
    {
        if (_orderService == null)
            return new List<Order>();

        try
        {
            return _orderService.GetCustomerOrders(customerId);
        }
        catch
        {
            return new List<Order>();
        }
    }

    public Order? GetOrderDetails(int orderId, int customerId)
    {
        if (_orderService == null)
            return null;

        try
        {
            var orders = _orderService.GetCustomerOrders(customerId);
            return orders.FirstOrDefault(o =>
            {
                var id = GetPrivatePropertyValue<int>(o, "Orderid");
                return id == orderId;
            });
        }
        catch
        {
            return null;
        }
    }

    public OrderStatus? GetOrderStatus(int orderId, int customerId)
    {
        if (_orderService == null)
            return null;

        try
        {
            var orders = _orderService.GetCustomerOrders(customerId);
            var order = orders.FirstOrDefault(o =>
            {
                var id = GetPrivatePropertyValue<int>(o, "Orderid");
                return id == orderId;
            });

            if (order == null) return null;

            // Try to get status from the order object
            var status = GetPrivatePropertyValue<OrderStatus>(order, "OrderStatus");
            return status;
        }
        catch
        {
            return null;
        }
    }

    public bool IsOrderCancellable(int orderId, int customerId)
    {
        var status = GetOrderStatus(orderId, customerId);
        return status == OrderStatus.PENDING || status == OrderStatus.CONFIRMED;
    }

    public bool CancelOrder(int orderId, int customerId)
    {
        if (_orderService == null)
            return false;

        if (!IsOrderCancellable(orderId, customerId))
            return false;

        try
        {
            return _orderService.CancelOrder(orderId, customerId);
        }
        catch
        {
            return false;
        }
    }

    public Customer? GetCustomerInformation(int customerId)
    {
        if (_customerService == null)
            return null;

        try
        {
            return _customerService.GetCustomerInformation(customerId);
        }
        catch
        {
            return null;
        }
    }

    // Helper method to get OrderId from order (for use in views)
    public int GetOrderId(Order order)
    {
        if (order == null) return 0;
        return GetPrivatePropertyValue<int>(order, "Orderid");
    }

    // Helper method to get OrderDate from order
    public DateTime GetOrderDate(Order order)
    {
        if (order == null) return DateTime.MinValue;
        return GetPrivatePropertyValue<DateTime>(order, "Orderdate");
    }

    // Helper method to get TotalAmount from order
    public decimal GetTotalAmount(Order order)
    {
        if (order == null) return 0;
        return GetPrivatePropertyValue<decimal>(order, "Totalamount");
    }

    // Helper method to get OrderStatus from order
    public OrderStatus GetOrderStatusFromOrder(Order order)
    {
        if (order == null) return OrderStatus.PENDING;
        return GetPrivatePropertyValue<OrderStatus>(order, "OrderStatus");
    }

    public List<Refund> GetCustomerRefunds(int customerId)
    {
        if (_refundService == null)
            return new List<Refund>();

        return _refundService.GetCustomerRefunds(customerId);
    }

    public List<Returnrequest> GetCustomerReturns(int customerId)
    {
        return new List<Returnrequest>();
    }

    public List<Notification> GetCustomerNotifications(int customerId, bool unreadOnly = false)
    {
        return new List<Notification>();
    }

    public Notificationpreference? GetNotificationPreferences(int customerId)
    {
        return null;
    }

    public void UpdateNotificationPreferences(int customerId, bool emailEnabled, bool smsEnabled)
    {
        // TODO: Implement when notification service is available
    }

    // Helper methods for refund properties
    public int GetRefundId(Refund refund)
    {
        if (refund == null) return 0;
        return GetPrivatePropertyValue<int>(refund, "Refundid");
    }

    public int GetRefundOrderId(Refund refund)
    {
        if (refund == null) return 0;
        return GetPrivatePropertyValue<int>(refund, "Orderid");
    }

    public DateTime GetRefundDate(Refund refund)
    {
        if (refund == null) return DateTime.MinValue;
        return GetPrivatePropertyValue<DateTime>(refund, "Returndate");
    }

    public decimal GetRefundAmount(Refund refund)
    {
        if (refund == null) return 0;
        return GetPrivatePropertyValue<decimal>(refund, "Depositrefundamount");
    }

    public string GetRefundMethod(Refund refund)
    {
        if (refund == null) return "Unknown";
        return GetPrivatePropertyValue<string>(refund, "Returnmethod") ?? "Unknown";
    }
}