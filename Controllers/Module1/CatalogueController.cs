using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module6.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Module6.Entities;

namespace ProRental.Controllers.Module1.P2_6;

public class CatalogueController : Controller
{
    private readonly CatalogueControl _catalogueControl;

    public CatalogueController(CatalogueControl catalogueControl)
    {
        _catalogueControl = catalogueControl;
    }

    // GET: /Catalogue
    public IActionResult Index()
    {
        var filter = new SearchFilter();

        var pagedProducts = _catalogueControl.SearchProducts(filter);

        var vm = new CatalogueViewModel
        {
            PagedProducts = pagedProducts,
            Filter = filter,
            Availability = new Dictionary<int, AvailabilityStatus>(),
            CarbonData = new Dictionary<int, CarbonFootprint?>()
        };

        // populate availability + carbon
        foreach (var p in vm.PagedProducts.Items)
        {
            vm.Availability[p.GetProductId()] = _catalogueControl.GetAvailability(p.GetProductId());
            vm.CarbonData[p.GetProductId()] = _catalogueControl.GetCarbonFootprint(p.GetProductId());
        }

        return View("~/Views/Module1/P2-6/CatalogBrowsing.cshtml", vm);
    }
}