using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;
using ProRental.Domain.Entities;

namespace ProRental.Controllers.Module1;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetOrCreateActiveCartBySession("test-session");

        var items = _cartService.GetCartDisplaySummary(cart.GetCartId());

        // TEMP: inject product
        foreach (var item in items)
        {
            var product = new Product();
            product.SetName($"Product {item.GetProductId()}");
            product.SetPrice(10 * item.GetProductId());

            item.SetProduct(product);
        }

return View("~/Views/Module1/P2-6/Cart.cshtml", items);
}
}