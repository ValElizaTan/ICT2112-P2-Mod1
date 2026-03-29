using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class CustomerDashboardController : Controller
{
    private readonly CustomerDashboardControl _control;
    private readonly RefundControl _refundControl;

    public CustomerDashboardController(CustomerDashboardControl control, RefundControl refundControl)
    {
        _control = control;
        _refundControl = refundControl;
    }

    private bool IsCustomer()
    {
        var role = HttpContext.Session.GetString("UserRole");
        return !string.IsNullOrEmpty(role) &&
               role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase);
    }

    [HttpGet]
    public IActionResult Index(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var allOrders = _control.GetCustomerOrders(customerId);
        var customer = _control.GetCustomerInformation(customerId);

        // Get return request order IDs to filter them out
        var returns = _control.GetCustomerReturns(customerId);
        var returnOrderIds = new HashSet<int>(returns.Select(r =>
        {
            var field = r.GetType().GetField("_orderid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field != null ? (int)(field.GetValue(r) ?? 0) : 0;
        }));

        var orders = allOrders?.Where(o => !returnOrderIds.Contains(o.OrderId)).ToList();

        // Calculate stats using reflection helper methods
        int activeOrders = 0;
        int completedOrders = 0;
        decimal totalSpent = 0;

        if (orders != null)
        {
            foreach (var order in orders)
            {
                var status = _control.GetOrderStatusFromOrder(order);
                var amount = _control.GetTotalAmount(order);

                if (status == OrderStatus.DELIVERED)
                    completedOrders++;
                else if (status != OrderStatus.CANCELLED)
                    activeOrders++;

                totalSpent += amount;
            }
        }

        ViewData["Orders"] = orders;
        ViewData["Customer"] = customer;
        ViewData["ActiveOrdersCount"] = activeOrders;
        ViewData["CompletedOrdersCount"] = completedOrders;
        ViewData["TotalSpent"] = totalSpent;
        ViewData["Control"] = _control; // Pass control to view for helper methods

        return View();
    }

    [HttpGet]
    public IActionResult OrderDetails(int orderId, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var order = _control.GetOrderDetails(orderId, customerId);
        var status = order != null ? _control.GetOrderStatusFromOrder(order) : (OrderStatus?)null;
        var canCancel = order != null && _control.IsOrderCancellable(orderId, customerId);

        // Check if a return has already been initiated for this order
        var hasReturn = _refundControl.GetRefundByOrderId(orderId) != null
                        || _control.GetCustomerReturns(customerId)
                            .Any(r =>
                            {
                                var f = r.GetType().GetField("_orderid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                return f != null && (int)(f.GetValue(r) ?? 0) == orderId;
                            });

        ViewData["Order"] = order;
        ViewData["OrderStatus"] = status;
        ViewData["CanCancel"] = canCancel;
        ViewData["HasReturn"] = hasReturn;
        ViewData["CustomerId"] = customerId;
        ViewData["Control"] = _control;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelOrder(int orderId, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var success = _control.CancelOrder(orderId, customerId);

        if (success)
        {
            TempData["SuccessMessage"] = $"Order #{orderId} has been cancelled successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Unable to cancel order. Orders can only be cancelled if they are pending or confirmed.";
        }

        return RedirectToAction(nameof(OrderDetails), new { orderId, customerId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReturnOrder(int orderId, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        try
        {
            var order = _control.GetOrderDetails(orderId, customerId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction(nameof(OrderDetails), new { orderId, customerId });
            }

            _refundControl.InitiateReturn(orderId, customerId, "Self-Ship");

            TempData["SuccessMessage"] = $"Return initiated for Order #{orderId}. Items will be inspected and deposit refund will be processed upon completion.";
        }
        catch (Exception ex)
        {
            var innerMsg = ex.InnerException?.Message ?? ex.Message;
            TempData["ErrorMessage"] = $"Failed to initiate return: {innerMsg}";
        }

        return RedirectToAction(nameof(OrderDetails), new { orderId, customerId });
    }

    [HttpGet]
    public IActionResult TrackOrder(int orderId, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");
        return RedirectToAction("CustomerOrderTracking", "OrderTracking", new { customerId, timelineOrderId = orderId });
    }

    [HttpGet]
    public IActionResult Returns(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var customerReturns = _control.GetCustomerReturns(customerId);
        var allOrders = _control.GetCustomerOrders(customerId);

        // Build a lookup of orders by ID for showing order details alongside returns
        var orderLookup = allOrders?.ToDictionary(o => o.OrderId) ?? new Dictionary<int, ProRental.Domain.Entities.Order>();

        ViewData["Returns"] = customerReturns;
        ViewData["OrderLookup"] = orderLookup;
        ViewData["CustomerId"] = customerId;
        ViewData["Control"] = _control;
        return View();
    }

    [HttpGet]
    public IActionResult Refunds(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var refunds = _control.GetCustomerRefunds(customerId);
        var refundDisplayList = refunds.Select(r => new
        {
            RefundId = _control.GetRefundId(r),
            OrderId = _control.GetRefundOrderId(r),
            RefundDate = _control.GetRefundDate(r),
            Amount = _control.GetRefundAmount(r),
            Method = _control.GetRefundMethod(r)
        }).ToList<dynamic>();

        ViewData["Refunds"] = refundDisplayList;
        ViewData["CustomerId"] = customerId;
        return View();
    }

    [HttpGet]
    public IActionResult Notifications(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var notifications = _control.GetCustomerNotifications(customerId);
        var preferences = _control.GetNotificationPreferences(customerId);

        ViewData["Notifications"] = notifications;
        ViewData["Preferences"] = preferences;
        ViewData["CustomerId"] = customerId;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdatePreferences(int customerId, bool emailEnabled, bool smsEnabled, NotificationFrequency notificationFrequency, NotificationGranularity notificationGranularity)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        _control.UpdateNotificationPreferences(customerId, emailEnabled, smsEnabled, notificationFrequency, notificationGranularity);
        TempData["SuccessMessage"] = "Notification preferences updated.";
        return RedirectToAction(nameof(Notifications), new { customerId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MarkAsRead(int id, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        _control.MarkNotificationAsRead(id, customerId);
        return RedirectToAction(nameof(Notifications), new { customerId });
    }
    public IActionResult OnNavigateToCustomerProfile(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        return RedirectToAction("Index", "CustomerProfile", new { customerId });
    }
}