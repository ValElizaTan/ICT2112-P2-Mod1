using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
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
    private readonly INotificationGateway _notificationGateway;
    private readonly INotificationPreferenceGateway _preferenceGateway;

    public CustomerDashboardControl(
        Team6.IOrderService orderService,
        INotificationGateway notificationGateway,
        INotificationPreferenceGateway preferenceGateway,
        ICustomerService? customerService = null,
        IRefundService? refundService = null)
    {
        _orderService = orderService;
        _notificationGateway = notificationGateway;
        _preferenceGateway = preferenceGateway;
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

    // Helper method to set private property value using reflection
    private void SetPrivateProperty(object obj, string propertyName, object value)
    {
        var property = obj.GetType().GetProperty(propertyName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (property == null) throw new InvalidOperationException($"Property '{propertyName}' not found on '{obj.GetType().Name}'");
        property.SetValue(obj, value);
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
        var notifications = _notificationGateway.FindByUser(customerId);
        if (unreadOnly)
        {
            return notifications.Where(n => !EF.Property<bool>(n, "Isread")).ToList();
        }
        return notifications;
    }

    public Notificationpreference? GetNotificationPreferences(int customerId)
    {
        var preference = _preferenceGateway.FindByUser(customerId);
        if (preference != null)
            return preference;

        preference = new Notificationpreference();
        SetPrivateProperty(preference, "Userid", customerId);
        SetPrivateProperty(preference, "Emailenabled", true);
        SetPrivateProperty(preference, "Smsenabled", false);
        SetPrivateProperty(preference, "Notificationfrequency", NotificationFrequency.DAILY);
        SetPrivateProperty(preference, "NotificationGranularity", NotificationGranularity.ALL);

        return preference;
    }

    public void UpdateNotificationPreferences(int customerId, bool emailEnabled, bool smsEnabled, NotificationFrequency frequency, NotificationGranularity granularity)
    {
        var preference = _preferenceGateway.FindByUser(customerId);

        if (preference == null)
        {
            preference = new Notificationpreference();
            SetPrivateProperty(preference, "Userid", customerId);
        }

        SetPrivateProperty(preference, "Emailenabled", emailEnabled);
        SetPrivateProperty(preference, "Smsenabled", smsEnabled);
        SetPrivateProperty(preference, "Notificationfrequency", frequency);
        SetPrivateProperty(preference, "NotificationGranularity", granularity);

        var preferenceId = GetPrivatePropertyValue<int>(preference, "Preferenceid");
        if (preferenceId == 0)
        {
            _preferenceGateway.InsertPreference(preference);
        }
        else
        {
            _preferenceGateway.UpdatePreference(preference);
        }
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