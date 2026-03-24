using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module6.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Module6.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1.P2_6;

public class CatalogueController : Controller
{
    private readonly CatalogueControl _catalogueControl;
    private readonly ICartService     _cartService;

    public CatalogueController(CatalogueControl catalogueControl, ICartService cartService)
    {
        _catalogueControl = catalogueControl;
        _cartService      = cartService;
    }

    // GET: /Catalogue
    public IActionResult Index(string? keywords, int? categoryId,
                               decimal? minPrice, decimal? maxPrice,
                               string? sortBy, int page = 1)
    {
        var filter = new SearchFilter
        {
            Keywords    = keywords,
            CategoryId  = categoryId,
            MinPrice    = minPrice,
            MaxPrice    = maxPrice,
            SortBy      = sortBy ?? "name_asc",
            CurrentPage = page           // ✅ was "Page", correct property is "CurrentPage"
        };

        var pagedProducts = _catalogueControl.SearchProducts(filter);

        var vm = new CatalogueViewModel
        {
            PagedProducts = pagedProducts,
            Filter        = filter,
            Categories    = _catalogueControl.GetCategories(), // ✅ added below
            Availability  = new Dictionary<int, AvailabilityStatus>(),
            CarbonData    = new Dictionary<int, CarbonFootprint?>()
        };

        foreach (var p in vm.PagedProducts.Items)
        {
            vm.Availability[p.GetProductId()] = _catalogueControl.GetAvailability(p.GetProductId());
            vm.CarbonData[p.GetProductId()]   = _catalogueControl.GetCarbonFootprint(p.GetProductId());
        }

        return View("~/Views/Module1/P2-6/CatalogBrowsing.cshtml", vm);
    }

    // POST: /Catalogue/AddToOrder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToOrder(int productId, int quantity)
    {
        var cart   = _cartService.GetOrCreateActiveCartBySession("test-session");
        var cartId = cart.GetCartId();

        if (cartId == null)
        {
            TempData["Error"] = "Could not resolve cart. Please try again.";
            return RedirectToAction("Index");
        }

        _cartService.AddToCart(cartId, productId, quantity);

        var product = _catalogueControl.GetProductById(productId); // ✅ was "GetProduct"
        TempData["AddedItem"] = product?.GetProductName() ?? $"Product {productId}";
        TempData["AddedQty"]  = quantity;

        return RedirectToAction("Index");
    }

    // GET: /Catalogue/Detail/5
    public IActionResult Detail(int id)
    {
        var product = _catalogueControl.GetProductById(id); // ✅ was "GetProduct"
        if (product == null) return NotFound();

        return View("~/Views/Module1/P2-6/CatalogDetail.cshtml", product);
    }
}