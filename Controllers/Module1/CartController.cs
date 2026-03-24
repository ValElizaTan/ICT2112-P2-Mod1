using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // GET: /Cart
    public IActionResult Index()
    {
        var cart   = _cartService.GetOrCreateActiveCartBySession("test-session");
        var cartId = cart.GetCartId();

        if (cartId == null)
            return View("~/Views/Module1/P2-6/Cart.cshtml", new List<CartItem>());

        var items = _cartService.GetCartDisplaySummary(cartId);

        // TEMP: inject product stub until inventory service is ready
        foreach (var item in items)
        {
            var product = new Product();
            product.SetName($"Product {item.GetProductId()}");
            product.SetPrice(10 * item.GetProductId());
            item.SetProduct(product);
        }

        return View("~/Views/Module1/P2-6/Cart.cshtml", items);
    }

    // POST: /Cart/UpdateCart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCart(
        List<int>? selectedItems,
        Dictionary<int, int>? quantities,
        int? removeItemId,
        string? action)
    {
        var cart   = _cartService.GetOrCreateActiveCartBySession("test-session");
        var cartId = cart.GetCartId();

        if (cartId == null)
            return RedirectToAction("Index");

        // Remove single item
        if (removeItemId.HasValue)
        {
            _cartService.RemoveItem(cartId, removeItemId.Value);
            return RedirectToAction("Index");
        }

        switch (action)
        {
            case "update":
                if (quantities != null)
                    foreach (var (productId, qty) in quantities)
                        _cartService.UpdateQuantity(cartId, productId, qty);
                if (selectedItems != null)
                    foreach (var id in selectedItems)
                        _cartService.ToggleSelectItem(cartId, id, true);
                break;

            case "clear":
                _cartService.ClearSelection(cartId);
                break;

            case "checkout":
                if (_cartService.CanProceedToCheckout(cartId))
                    return RedirectToAction("Index", "Checkout");
                TempData["Error"] = "No items selected for checkout.";
                break;
        }

        return RedirectToAction("Index");
    }
}