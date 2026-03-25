using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class CustomerDashboardController : Controller
{
    private readonly CustomerDashboardControl _control;

    public CustomerDashboardController(CustomerDashboardControl control)
    {
        _control = control;
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

        var orders = _control.GetCustomerOrders(customerId);
        var customer = _control.GetCustomerInformation(customerId);

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

        ViewData["Order"] = order;
        ViewData["OrderStatus"] = status;
        ViewData["CanCancel"] = canCancel;
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

    [HttpGet]
    public IActionResult TrackOrder(int orderId, int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var order = _control.GetOrderDetails(orderId, customerId);
        var status = order != null ? _control.GetOrderStatusFromOrder(order) : (OrderStatus?)null;

        ViewData["Order"] = order;
        ViewData["OrderStatus"] = status;
        ViewData["CustomerId"] = customerId;
        ViewData["Control"] = _control;

        return View();
    }

    [HttpGet]
    public IActionResult Returns(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var returns = _control.GetCustomerReturns(customerId);
        ViewData["Returns"] = returns;
        ViewData["CustomerId"] = customerId;
        return View();
    }

    [HttpGet]
    public IActionResult Refunds(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        var refunds = _control.GetCustomerRefunds(customerId);
        ViewData["Refunds"] = refunds;
        ViewData["CustomerId"] = customerId;
        ViewData["Control"] = _control;
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
    public IActionResult UpdatePreferences(int customerId, bool emailEnabled, bool smsEnabled)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        _control.UpdateNotificationPreferences(customerId, emailEnabled, smsEnabled);
        TempData["SuccessMessage"] = "Notification preferences updated.";
        return RedirectToAction(nameof(Notifications), new { customerId });
    }
    public IActionResult OnNavigateToCustomerProfile(int customerId)
    {
        if (!IsCustomer()) return RedirectToAction("Login", "Module1");

        return RedirectToAction("Index", "CustomerProfile", new { customerId });
    }
}