using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICheckoutService _checkoutService;
    private readonly ICustomerValidationService _customerValidationService;
    private readonly ISessionService _sessionService;

    public CartController(
        ICartService cartService,
        ICheckoutService checkoutService,
        ICustomerValidationService customerValidationService,
        ISessionService sessionService)
    {
        _cartService = cartService;
        _checkoutService = checkoutService;
        _customerValidationService = customerValidationService;
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue && TempData["Error"]?.ToString() == "Please log in before proceeding to checkout.")
        {
            TempData.Remove("Error");
        }

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        var cart = _cartService.GetCart(cartId);

        ViewBag.CartId = cartId;
        ViewBag.Items = _cartService.GetCartDisplayItems(cartId);
        ViewBag.Summary = _cartService.GetCartDisplaySummary(cartId);
        ViewBag.GuestMode = !customerId.HasValue;

        ViewBag.RentalStart = cart.GetRentalStart() == DateTime.MinValue
            ? DateTime.Today.ToString("yyyy-MM-dd")
            : cart.GetRentalStart().ToString("yyyy-MM-dd");

        ViewBag.RentalEnd = cart.GetRentalEnd() == DateTime.MinValue
            ? DateTime.Today.ToString("yyyy-MM-dd")
            : cart.GetRentalEnd().ToString("yyyy-MM-dd");

        return View("~/Views/Module1/P2-6/Cart.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetRentalPeriod(DateTime start, DateTime end)
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        var warnings = _cartService.SetRentalPeriod(cartId, start.Date, end.Date);

        if (warnings != null && warnings.Count > 0)
        {
            TempData["Error"] = string.Join(" ", warnings);
        }
        else
        {
            TempData["Message"] = "Rental period updated.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleSelectItem(int productId, bool isSelected)
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.ToggleSelectItem(cartId, productId, isSelected);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectAll()
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.SelectAllObtainable(cartId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearSelection()
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.ClearSelection(cartId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int qty)
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.UpdateQuantity(cartId, productId, qty);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int productId)
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.RemoveItem(cartId, productId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EmptyCart()
    {
        int cartId;

        int? customerId = HttpContext.Session.GetInt32("CustomerId");
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (customerId.HasValue)
        {
            if (sessionId.HasValue)
            {
                cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
            }
            else
            {
                cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
            }
        }
        else
        {
            if (!sessionId.HasValue)
            {
                sessionId = 1; // local test only
                HttpContext.Session.SetInt32("SessionId", sessionId.Value);
            }

            cartId = _cartService.GetOrCreateActiveCartIdBySession(sessionId.Value);
        }

        _cartService.EmptyCart(cartId);
        TempData["Message"] = "Cart emptied.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartCheckout()
    {
        int? customerId = HttpContext.Session.GetInt32("CustomerId");

        if (!customerId.HasValue)
        {
            TempData["Error"] = "Please log in before proceeding to checkout.";
            return RedirectToAction("Login", "Module1");
        }

        var validation = _customerValidationService.ValidateCustomer(customerId.Value);
        if (!validation.IsValid)
        {
            TempData["Error"] = validation.ValidationMessage ?? "Please log in before proceeding to checkout.";
            return RedirectToAction(nameof(Index));
        }

        int cartId;
        int? sessionId = HttpContext.Session.GetInt32("SessionId");

        if (sessionId.HasValue)
        {
            cartId = _cartService.MergeSessionCartToCustomerCart(sessionId.Value, customerId.Value);
        }
        else
        {
            cartId = _cartService.GetOrCreateActiveCartIdByCustomer(customerId.Value);
        }

        if (!_cartService.CanProceedToCheckout(cartId))
        {
            TempData["Error"] = "Please select at least 1 valid item and complete the cart details before checkout.";
            return RedirectToAction(nameof(Index));
        }

        var selectedProductIds = _cartService.GetSelectedItems(cartId)
            .Select(x => x.GetProductId())
            .ToList();

        var checkoutId = _checkoutService.StartCheckout(customerId.Value, selectedProductIds);
        return RedirectToAction("Index", "Checkout", new { checkoutId });
    }
}