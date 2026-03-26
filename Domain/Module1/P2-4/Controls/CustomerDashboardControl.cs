using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using System.Reflection;
using Team6 = ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module1.P24.Controls;

public class CustomerDashboardControl
{
    private readonly Team6.IOrderService _orderService;
    private readonly ICustomerService? _customerService;
    private readonly IRefundService? _refundService;

    public CustomerDashboardControl(
        Team6.IOrderService orderService,
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
        try
        {
            return _orderService.GetOrdersByCustomer(customerId);
        }
        catch
        {
            return new List<Order>();
        }
    }

    public Order? GetOrderDetails(int orderId, int customerId)
    {
        try
        {
            return _orderService.GetOrder(orderId);
        }
        catch
        {
            return null;
        }
    }

    public OrderStatus? GetOrderStatus(int orderId, int customerId)
    {
        try
        {
            return _orderService.GetOrderStatus(orderId);
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
        if (!IsOrderCancellable(orderId, customerId))
            return false;

        try
        {
            return _orderService.CancelOrder(orderId);
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

    public int GetOrderId(Order order)
    {
        if (order == null) return 0;
        return order.OrderId;
    }

    public DateTime GetOrderDate(Order order)
    {
        if (order == null) return DateTime.MinValue;
        return order.OrderDate;
    }

    public decimal GetTotalAmount(Order order)
    {
        if (order == null) return 0;
        return order.TotalAmount;
    }

    public OrderStatus GetOrderStatusFromOrder(Order order)
    {
        if (order == null) return OrderStatus.PENDING;
        return order.CurrentStatus ?? OrderStatus.PENDING;
    }

    public List<Refund> GetCustomerRefunds(int customerId)
    {
        if (_refundService == null)
            return new List<Refund>();

        return _refundService.GetCustomerRefunds(customerId);
    }

    public List<Returnrequest> GetCustomerReturns(int customerId)
    {
        if (_refundService == null)
            return new List<Returnrequest>();

        try
        {
            return _refundService.GetCustomerReturns(customerId);
        }
        catch
        {
            return new List<Returnrequest>();
        }
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