using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetCartId()
    {
        return _cartService.GetOrCreateActiveCartIdByCustomer(1);
    }

    [HttpGet]
    public IActionResult Index()
    {
        int cartId = GetCartId();
        var cart = _cartService.GetCart(cartId);

        ViewBag.CartId = cartId;
        ViewBag.Items = _cartService.GetCartDisplayItems(cartId);
        ViewBag.Summary = _cartService.GetCartDisplaySummary(cartId);

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
        var today = DateTime.Today;

        if (start.Date < today)
        {
            TempData["Error"] = "Rental start date cannot be earlier than today.";
            return RedirectToAction(nameof(Index));
        }

        if (end.Date < today)
        {
            TempData["Error"] = "Rental end date cannot be earlier than today.";
            return RedirectToAction(nameof(Index));
        }

        if (end.Date < start.Date)
        {
            TempData["Error"] = "Rental end date cannot be earlier than start date.";
            return RedirectToAction(nameof(Index));
        }

        _cartService.SetRentalPeriod(GetCartId(), start.Date, end.Date);
        TempData["Message"] = "Rental period updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleSelectItem(int productId, bool isSelected)
    {
        _cartService.ToggleSelectItem(GetCartId(), productId, isSelected);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectAll()
    {
        _cartService.SelectAllObtainable(GetCartId());
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearSelection()
    {
        _cartService.ClearSelection(GetCartId());
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int qty)
    {
        _cartService.UpdateQuantity(GetCartId(), productId, qty);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int productId)
    {
        _cartService.RemoveItem(GetCartId(), productId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EmptyCart()
    {
        _cartService.EmptyCart(GetCartId());
        TempData["Message"] = "Cart emptied.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartCheckout()
    {
        int cartId = GetCartId();
        var cart = _cartService.GetCart(cartId);

        var start = cart.GetRentalStart();
        var end = cart.GetRentalEnd();
        var selectedItems = _cartService.GetSelectedItems(cartId);

        if (start == DateTime.MinValue || end == DateTime.MinValue)
        {
            TempData["Error"] = "Please select both rental start and end dates.";
            return RedirectToAction(nameof(Index));
        }

        if (end < start)
        {
            TempData["Error"] = "End rental date should not be earlier than start rental date.";
            return RedirectToAction(nameof(Index));
        }

        if (selectedItems == null || selectedItems.Count == 0)
        {
            TempData["Error"] = "Please select at least 1 item to checkout.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Message"] = "Checkout is not implemented yet. Selected items will remain in the active cart flow.";
        return RedirectToAction(nameof(Index));
    }
}