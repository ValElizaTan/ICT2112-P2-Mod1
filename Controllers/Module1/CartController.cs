using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICheckoutService _checkoutService;
    private readonly ICustomerValidationService _customerValidationService;

    public CartController(
        ICartService cartService,
        ICheckoutService checkoutService,
        ICustomerValidationService customerValidationService)
    {
        _cartService = cartService;
        _checkoutService = checkoutService;
        _customerValidationService = customerValidationService;
    }

    private int? GetResolvedCustomerId()
    {
        return HttpContext.Session.GetInt32("CustomerId")
            ?? HttpContext.Session.GetInt32("ValidatedCustomerId");
    }

    private int GetCartId()
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId")
            ?? HttpContext.Session.GetInt32("ValidatedCustomerId");

        if (!customerId.HasValue)
        {
            throw new InvalidOperationException("Please log in to access your cart.");
        }

        return _cartService.GetOrCreateActiveCartIdByCustomerId(customerId.Value);
    }

    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            int cartId = GetCartId();
            var cart = _cartService.GetCart(cartId);

            ViewBag.CartId = cartId;
            ViewBag.Items = _cartService.GetCartDisplayItems(cartId);
            ViewBag.Summary = _cartService.GetCartDisplaySummary(cartId);
            ViewBag.RentalStart = cart.GetRentalStart() == DateTime.MinValue
                ? ""
                : cart.GetRentalStart().ToString("yyyy-MM-dd");
            ViewBag.RentalEnd = cart.GetRentalEnd() == DateTime.MinValue
                ? ""
                : cart.GetRentalEnd().ToString("yyyy-MM-dd");

            return View("~/Views/Module1/P2-6/Cart.cshtml");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SetRentalPeriod(DateTime start, DateTime end)
    {
        try
        {
            int cartId = GetCartId();
            var warnings = _cartService.SetRentalPeriod(cartId, start, end);

            if (warnings.Any())
            {
                TempData["Error"] = string.Join(" ", warnings);
            }
            else
            {
                TempData["Message"] = "Rental period updated.";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleSelectItem(int productId, bool isSelected)
    {
        try
        {
            int cartId = GetCartId();
            _cartService.ToggleSelectItem(cartId, productId, isSelected);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectAll()
    {
        try
        {
            int cartId = GetCartId();
            _cartService.SelectAllObtainable(cartId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearSelection()
    {
        try
        {
            int cartId = GetCartId();
            _cartService.ClearSelection(cartId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int qty)
    {
        try
        {
            int cartId = GetCartId();
            _cartService.UpdateQuantity(cartId, productId, qty);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int productId)
    {
        try
        {
            int cartId = GetCartId();
            _cartService.RemoveItem(cartId, productId);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EmptyCart()
    {
        try
        {
            int cartId = GetCartId();
            _cartService.EmptyCart(cartId);
            TempData["Message"] = "Cart emptied.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartCheckout()
    {
        try
        {
            var customerId = GetResolvedCustomerId();
            if (!customerId.HasValue)
            {
                TempData["Error"] = "Please complete customer validation before proceeding to checkout.";
                return RedirectToAction(nameof(Index));
            }

            var validation = _customerValidationService.ValidateCustomer(customerId.Value);
            if (!validation.IsValid)
            {
                TempData["Error"] = validation.ValidationMessage ?? "Customer is not allowed to checkout.";
                return RedirectToAction(nameof(Index));
            }

            int cartId = GetCartId();

            if (!_cartService.CanProceedToCheckout(cartId))
            {
                TempData["Error"] = "Please select at least one item and complete cart details before checkout.";
                return RedirectToAction(nameof(Index));
            }

            var selectedProductIds = _cartService.GetSelectedItems(cartId)
                .Select(x => x.GetProductId())
                .ToList();

            var checkoutId = _checkoutService.StartCheckout(customerId.Value, selectedProductIds);

            return RedirectToAction("Index", "Checkout", new { checkoutId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}