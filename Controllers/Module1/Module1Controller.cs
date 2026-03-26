using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;
using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;

namespace ProRental.Controllers.Module1;

/// <summary>
/// ASP.NET HTTP controller for Module 1 (authentication, session, customer validation).
/// This is a thin HTTP boundary only — all business logic lives in the Control classes.
/// </summary>
public class Module1Controller : Controller
{
    private readonly AuthenticationControl _authControl;
    private readonly CustomerIDValidationControl _customerIdValidationControl;
    private readonly IOrderService _orderService;
    private readonly IShippingOptionService _shippingService;
    private readonly ICustomerGateway _customerGateway;

    public Module1Controller(
        AuthenticationControl authControl,
        CustomerIDValidationControl customerIdValidationControl,
        IOrderService orderService,
        IShippingOptionService shippingService,
        ICustomerGateway customerGateway)
    {
        _authControl = authControl;
        _customerIdValidationControl = customerIdValidationControl;
        _orderService = orderService;
        _shippingService = shippingService;
        _customerGateway = customerGateway;
    }

    // ── Login ────────────────────────────────────────────────────────────

    // GET /Module1/Login
    public IActionResult Login() => View("P2-6/Login");

    // POST /Module1/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password)
    {
        var result = _authControl.AuthenticateUser(email, password);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");
            return View("P2-6/Login");
        }

        HttpContext.Session.SetInt32("SessionId", result.Session!.SessionId);
        HttpContext.Session.SetInt32("UserId", result.Session.UserId);
        HttpContext.Session.SetString("UserName", result.UserName ?? email);
        HttpContext.Session.SetString("UserRole", result.Session.RoleString);

        var customer = _customerGateway.FindByEmail(email);
        if (customer != null)
            HttpContext.Session.SetInt32("CustomerId", customer.GetCustomerInfo().CustomerId);

        return RedirectToAction("CustomerLoginSuccess");
    }

    // POST /Module1/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var sessionId = HttpContext.Session.GetInt32("SessionId");
        if (sessionId.HasValue)
            _authControl.Logout(sessionId.Value);

        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // ── Customer ID Validation ───────────────────────────────────────────

    // GET /Module1/CustomerIdEntry
    public IActionResult CustomerIdEntry()
    {
        return View("P2-6/CustomerIdEntry");
    }

    // POST /Module1/CustomerIdEntry
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CustomerIdEntry(int customerId)
    {
        var result = _customerIdValidationControl.ValidateCustomer(customerId);

        if (!result.IsValid)
        {
            ViewBag.ValidationMessage = result.ValidationMessage;
            return View("P2-6/CustomerIdEntry");
        }

        HttpContext.Session.SetInt32("ValidatedCustomerId", result.CustomerId);
        return RedirectToAction("Index", "Cart");
    }

    // ── Customer Login Success ───────────────────────────────────────────

    // GET /Module1/CustomerLoginSuccess
    public IActionResult CustomerLoginSuccess()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role) ||
            !role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Login");
        }

        var viewPath = "P2-6/CustomerLoginSuccess";
        return ViewExists(viewPath)
            ? View(viewPath)
            : RedirectToAction("Index", "Home");
    }

    // ── Staff Login ──────────────────────────────────────────────────────

    // GET /Module1/StaffLogin
    public IActionResult StaffLogin()
    {
        ViewBag.ActiveTab = "staff";
        return View("P2-6/Login");
    }

    // POST /Module1/StaffLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StaffLogin(string StaffEmail, string StaffPassword)
    {
        var result = _authControl.AuthenticateUser(StaffEmail, StaffPassword);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Staff login failed.");
            ViewBag.ActiveTab = "staff";
            return View("P2-6/Login");
        }

        var roleString = result.Session!.RoleString;
        if (!Enum.TryParse<UserRole>(roleString, ignoreCase: true, out var role) ||
            (role != UserRole.STAFF && role != UserRole.ADMIN))
        {
            ModelState.AddModelError(string.Empty,
                "Access denied. This portal is for staff and administrators only.");
            ViewBag.ActiveTab = "staff";
            return View("P2-6/Login");
        }

        HttpContext.Session.SetInt32("SessionId", result.Session.SessionId);
        HttpContext.Session.SetInt32("UserId", result.Session.UserId);
        HttpContext.Session.SetString("UserName", result.UserName ?? StaffEmail);
        HttpContext.Session.SetString("UserEmail", StaffEmail);
        HttpContext.Session.SetString("UserRole", roleString);

        return RedirectToAction("StaffLoginSuccess");
    }

    // GET /Module1/StaffLoginSuccess
    public IActionResult StaffLoginSuccess()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role) ||
            (!role.Equals("STAFF", StringComparison.OrdinalIgnoreCase) &&
             !role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase)))
        {
            return RedirectToAction("StaffLogin");
        }

        return View("P2-6/StaffLoginSuccess");
    }

    // ── Signup ───────────────────────────────────────────────────────────

    // GET /Module1/Signup
    public IActionResult Signup()
    {
        return View("P2-6/Signup");
    }

    // POST /Module1/Signup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(
        string firstName,
        string lastName,
        string email,
        string? phone,
        string password,
        string confirmPassword,
        bool agreeTerms)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return View("P2-6/Signup");
        }

        if (!agreeTerms)
        {
            ModelState.AddModelError(string.Empty, "You must agree to the Terms of Service and Privacy Policy.");
            return View("P2-6/Signup");
        }

        // TODO: wire up SignupControl here when backend is ready.

        TempData["SignupName"] = firstName;
        return RedirectToAction("SignupSuccess");
    }

    // GET /Module1/SignupSuccess
    public IActionResult SignupSuccess()
    {
        return View("P2-6/SignupSuccess");
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private bool ViewExists(string viewPath)
    {
        var result = HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine>()
            .FindView(ControllerContext, viewPath, isMainPage: false);

        return result.Success;
    }

    // ── Order Management ─────────────────────────────────────────────────

    // GET /Module1/Orders?status=all
    public IActionResult Orders(string status = "all")
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId")
                      ?? HttpContext.Session.GetInt32("ValidatedCustomerId");

        if (customerId == null)
            return RedirectToAction("Login");

        var orders = _orderService.GetOrdersByCustomer(customerId.Value);

        var filtered = status.ToLower() switch
        {
            "pending"   => orders.Where(o => o.CurrentStatus == OrderStatus.PENDING).ToList(),
            "confirmed" => orders.Where(o => o.CurrentStatus == OrderStatus.CONFIRMED ||
                                             o.CurrentStatus == OrderStatus.PROCESSING).ToList(),
            "dispatch"  => orders.Where(o => o.CurrentStatus == OrderStatus.READY_FOR_DISPATCH ||
                                             o.CurrentStatus == OrderStatus.DISPATCHED).ToList(),
            "delivered" => orders.Where(o => o.CurrentStatus == OrderStatus.DELIVERED).ToList(),
            "cancelled" => orders.Where(o => o.CurrentStatus == OrderStatus.CANCELLED).ToList(),
            _           => orders
        };

        ViewBag.CustomerId     = customerId.Value;
        ViewBag.ActiveTab      = status;
        ViewBag.AllCount       = orders.Count;
        ViewBag.PendingCount   = orders.Count(o => o.CurrentStatus == OrderStatus.PENDING);
        ViewBag.ConfirmedCount = orders.Count(o => o.CurrentStatus == OrderStatus.CONFIRMED ||
                                                    o.CurrentStatus == OrderStatus.PROCESSING);
        ViewBag.DispatchCount  = orders.Count(o => o.CurrentStatus == OrderStatus.READY_FOR_DISPATCH ||
                                                    o.CurrentStatus == OrderStatus.DISPATCHED);
        ViewBag.DeliveredCount = orders.Count(o => o.CurrentStatus == OrderStatus.DELIVERED);
        ViewBag.CancelledCount = orders.Count(o => o.CurrentStatus == OrderStatus.CANCELLED);

        return View("P2-6/Orders", filtered);
    }

    // GET /Module1/OrderDetail/5
    public IActionResult OrderDetail(int id)
    {
        var order = _orderService.GetOrder(id);
        return View("P2-6/OrderDetail", order);
    }

    // POST /Module1/CancelOrder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelOrder(int orderId)
    {
        _orderService.CancelOrder(orderId);
        return RedirectToAction("Orders", new { status = "cancelled" });
    }

    // GET /Module1/CreateOrderTest
    public IActionResult CreateOrderTest()
    {
        return View("P2-6/CreateOrderTest");
    }

    // POST /Module1/CreateOrderTest
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateOrderTest(int customerId, int checkoutId,
        string deliveryType, decimal totalAmount,
        int productId1, int quantity1, decimal unitPrice1,
        int productId2, int quantity2, decimal unitPrice2)
    {
        var itemData = new List<(int, int, decimal, DateTime, DateTime)>
        {
            (productId1, quantity1, unitPrice1, DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
            (productId2, quantity2, unitPrice2, DateTime.UtcNow, DateTime.UtcNow.AddDays(7)),
        };

        var productQuantities = new Dictionary<int, int>
        {
            { productId1, quantity1 },
            { productId2, quantity2 }
        };

        var delivery = Enum.Parse<DeliveryDuration>(deliveryType);

        var order = _orderService.CreateOrder(customerId, checkoutId, 0, itemData,
                                               delivery, totalAmount, productQuantities);

        TempData["CreatedOrderId"]     = order.OrderId;
        TempData["CreatedOrderStatus"] = order.CurrentStatus?.ToString();

        return RedirectToAction("OrderDetail", new { id = order.OrderId });
    }
}
