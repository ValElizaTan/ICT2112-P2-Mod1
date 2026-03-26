using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

public class RefundReturnController : Controller
{
    private readonly RefundControl _refundControl;
    private readonly ICustomerService _customerService;

    public RefundReturnController(RefundControl refundControl, ICustomerService customerService)
    {
        _refundControl = refundControl;
        _customerService = customerService;
    }

    private bool IsStaff()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "";
        return role.Equals("STAFF", StringComparison.OrdinalIgnoreCase)
            || role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        var allOrders = _refundControl.GetAllOrders();

        var orderViewModels = allOrders.Select(o =>
        {
            var customerName = GetCustomerName(o.CustomerId);
            var status = o.CurrentStatus?.ToString() ?? "Unknown";
            var existingRefund = _refundControl.GetRefundByOrderId(o.OrderId);

            return new
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                CustomerName = customerName,
                OrderDate = o.OrderDate,
                Total = o.TotalAmount,
                ItemCount = o.Orderitems?.Count ?? 0,
                Status = status,
                HasRefund = existingRefund != null
            };
        }).ToList();

        ViewBag.Orders = orderViewModels;
        return View();
    }

    [HttpGet]
    public IActionResult InitiateReturn(int orderId)
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        var order = _refundControl.GetOrder(orderId);
        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction(nameof(Index));
        }

        var customerName = GetCustomerName(order.CustomerId);

        ViewBag.OrderId = order.OrderId;
        ViewBag.CustomerId = order.CustomerId;
        ViewBag.CustomerName = customerName;
        ViewBag.OrderDate = order.OrderDate;
        ViewBag.Total = order.TotalAmount;
        ViewBag.ItemCount = order.Orderitems?.Count ?? 0;
        ViewBag.Status = order.CurrentStatus?.ToString() ?? "Unknown";

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmReturn(int orderId, int customerId, string returnMethod)
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        try
        {
            _refundControl.InitiateReturn(orderId, customerId, returnMethod);
            TempData["SuccessMessage"] = $"Return initiated for Order #{orderId}. Inspection triggered — deposit refund will be processed upon completion.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            var innerMsg = ex.InnerException?.Message ?? ex.Message;
            TempData["ErrorMessage"] = $"Failed to initiate return: {innerMsg}";
            return RedirectToAction(nameof(InitiateReturn), new { orderId });
        }
    }

    private string GetCustomerName(int customerId)
    {
        try
        {
            var customer = _customerService.GetCustomerInformation(customerId);
            var info = customer?.GetCustomerInfo();
            return info?.User?.Name ?? $"Customer #{customerId}";
        }
        catch
        {
            return $"Customer #{customerId}";
        }
    }
}
