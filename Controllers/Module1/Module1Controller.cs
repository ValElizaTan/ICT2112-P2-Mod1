using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Controls;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

// ✅ ADD THIS
using ProRental.Controllers.Module1;

namespace ProRental.Controllers.Module1;

public class Module1Controller : Controller
{
    private readonly AuthenticationControl _authControl;
    private readonly CustomerIDValidationControl _customerIdValidationControl;

    // ✅ ADD THIS (Catalogue)
    private readonly CatalogueControl _catalogueControl = new();

    public Module1Controller(
        AuthenticationControl authControl,
        CustomerIDValidationControl customerIdValidationControl)
    {
        _authControl = authControl;
        _customerIdValidationControl = customerIdValidationControl;
    }

    // ── Login ───────────────────────────────────────────

    public IActionResult Login()
    {
        return View("P2-6/Login");
    }

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
        HttpContext.Session.SetString("UserName", result.UserName ?? email);
        HttpContext.Session.SetString("UserRole", result.Session.RoleString);

        return RedirectToAction("CustomerLoginSuccess");
    }

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

    // ── Customer ID Validation ───────────────────────────

    public IActionResult CustomerIdEntry()
    {
        return View("P2-6/_CustomerIdEntry");
    }

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

    // ── Customer Login Success ───────────────────────────

    public IActionResult CustomerLoginSuccess()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role) ||
            !role.Equals("CUSTOMER", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Login");
        }

        var viewPath = "P2-6/CustomerLoginSuccess";
        var viewExists = ViewExists(viewPath);

        return viewExists
            ? View(viewPath)
            : RedirectToAction("Index", "Home");
    }

    // ── Staff Login ───────────────────────────

    public IActionResult StaffLogin()
    {
        return View("P2-6/StaffLogin");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StaffLogin(string StaffEmail, string StaffPassword)
    {
        var result = _authControl.AuthenticateUser(StaffEmail, StaffPassword);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Staff login failed.");
            return View("P2-6/StaffLogin");
        }

        var roleString = result.Session!.RoleString;
        if (!Enum.TryParse<UserRole>(roleString, true, out var role) ||
            (role != UserRole.STAFF && role != UserRole.ADMIN))
        {
            ModelState.AddModelError(string.Empty,
                "Access denied. This portal is for staff and administrators only.");
            return View("P2-6/StaffLogin");
        }

        HttpContext.Session.SetInt32("SessionId", result.Session.SessionId);
        HttpContext.Session.SetString("UserName", result.UserName ?? StaffEmail);
        HttpContext.Session.SetString("UserRole", roleString);

        return RedirectToAction("StaffLoginSuccess");
    }

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

    // ── Signup ───────────────────────────

    public IActionResult Signup()
    {
        return View("P2-6/Signup");
    }

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
            ModelState.AddModelError(string.Empty, "You must agree to the Terms of Service.");
            return View("P2-6/Signup");
        }

        TempData["SignupName"] = firstName;
        return RedirectToAction("SignupSuccess");
    }

    public IActionResult SignupSuccess()
    {
        return View("P2-6/SignupSuccess");
    }

    // ── Catalogue (NEW FEATURE) ───────────────────────────

    public IActionResult Browse()
    {
        var products = _catalogueControl.GetAllProducts();
        return View("P2-6/Browse", products);
    }

    public IActionResult AddToCart(int productId, int quantity = 1)
    {
        var result = _catalogueControl.AddProductSelection(productId, quantity);
        return Content(result);
    }

    // ── Helpers ───────────────────────────

    private bool ViewExists(string viewPath)
    {
        var result = HttpContext.RequestServices
            .GetRequiredService<Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine>()
            .FindView(ControllerContext, viewPath, false);

        return result.Success;
    }
}