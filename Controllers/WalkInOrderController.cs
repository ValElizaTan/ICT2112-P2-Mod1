using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Module1.P24.Interfaces;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1.P24;

public class WalkInOrderController : Controller
{
    private readonly WalkInOrderControl _control;
    private readonly ICustomerService _customerService;
    private readonly IInventoryService _inventoryService;

    private string _customerEmail = string.Empty;                                                                                                  
    private string _customerName = string.Empty;                                                                                                   
    private string _customerAddress = string.Empty;

    public WalkInOrderController(WalkInOrderControl control, ICustomerService customerService, IInventoryService inventoryService)
    {
        _control = control;
        _customerService = customerService;
        _inventoryService = inventoryService;
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
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        return View();
    }

    [HttpGet]
    public IActionResult SelectExistingCustomer()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        var customers = _customerService.GetCustomers();
        return View(customers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectExistingCustomer(int customerId)
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
        var customer = _customerService.GetCustomerInformation(customerId);
        var info = customer.GetCustomerInfo();
        TempData["CustomerId"]      = info.CustomerId;
        TempData["CustomerName"]    = info.User.Name;
        TempData["CustomerEmail"]   = info.User.Email;
        TempData["CustomerAddress"] = info.Address;
        return RedirectToAction(nameof(SelectProducts));
    }

    [HttpGet]
    public IActionResult EnterCustomerDetails()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Module1");
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
        TempData.Keep("CustomerId");
        TempData.Keep("CustomerName");
        TempData.Keep("CustomerEmail");
        TempData.Keep("CustomerAddress");
        var products = _inventoryService.GetAllProducts() ?? new List<Product>();
        return View(products);
    }

    [HttpGet]
    public IActionResult ReviewOrder(List<int>? productIds)
    {
        TempData.Keep("CustomerId");
        TempData.Keep("CustomerName");
        TempData.Keep("CustomerEmail");
        TempData.Keep("CustomerAddress");

        if (productIds == null || productIds.Count == 0)
        {
            TempData["ErrorMessage"] = "Please select at least one product before proceeding.";
            return RedirectToAction(nameof(SelectProducts));
        }

        var selectedProducts = productIds
            .Select(id => _inventoryService.GetProduct(id))
            .Where(p => p != null)
            .ToList();

        ViewBag.ProductIds = productIds;
        return View(selectedProducts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmOrder(int customerId, List<int>? productIds, DeliveryType deliveryMethod)
    {
        try
        {
            if (productIds == null || productIds.Count == 0)
            {
                TempData["ErrorMessage"] = "Please select at least one product before confirming.";
                return RedirectToAction(nameof(SelectProducts));
            }

            var order = _control.CreateOrder(customerId, productIds, deliveryMethod);
            return DisplayOrderStatus(OrderStatus.CONFIRMED);
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
        return View("~/Views/Module1/P2-4/WalkInOrder/Error.cshtml");
    }
}
