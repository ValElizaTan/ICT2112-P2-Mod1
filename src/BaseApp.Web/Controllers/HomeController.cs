using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
