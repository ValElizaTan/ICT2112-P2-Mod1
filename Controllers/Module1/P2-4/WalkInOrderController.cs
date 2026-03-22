using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class WalkInOrderController : Controller
{
    private readonly WalkInOrderControl _control;

    private string _customerEmail = string.Empty;
    private string _customerName = string.Empty;
    private string _customerAddress = string.Empty;

    public WalkInOrderController(WalkInOrderControl control)
    {
        _control = control;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EnterCustomerDetails()
    {
        return View();
    }

    [HttpPost]
    public IActionResult EnterCustomerDetails(string customerEmail, string customerName, string customerAddress)
    {
        _customerEmail = customerEmail;
        _customerName = customerName;
        _customerAddress = customerAddress;

        TempData["CustomerEmail"] = customerEmail;
        TempData["CustomerName"] = customerName;
        TempData["CustomerAddress"] = customerAddress;

        return RedirectToAction(nameof(SelectProducts));
    }

    [HttpGet]
    public IActionResult SelectProducts()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ReviewOrder()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ConfirmOrder(int customerId, List<int> productIds, DeliveryType deliveryMethod)
    {
        try
        {
            // Build order items from selected product IDs (quantities assumed 1 for now)
            var items = productIds.Select(pid => new Orderitem()).ToList();

            var order = _control.CreateOrder(customerId, items, deliveryMethod);
            return DisplayOrderStatus(OrderStatus.PLACED);
        }
        catch (Exception ex)
        {
            return DisplayError(ex.Message);
        }
    }

    private IActionResult DisplayOrderStatus(OrderStatus status)
    {
        ViewData["OrderStatus"] = status;
        return View("OrderStatus");
    }

    private IActionResult DisplayError(string message)
    {
        ViewData["ErrorMessage"] = message;
        return View("Error");
    }
}
