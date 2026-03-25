using Microsoft.AspNetCore.Mvc;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CheckoutController : Controller
{
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpGet]
    public IActionResult Index(int checkoutId)
    {
        try
        {
            var checkout = _checkoutService.GetCheckout(checkoutId);
            var cart = _checkoutService.GetSelectedCartSnapshot(checkoutId);
            var summary = _checkoutService.GetCostSummary(checkoutId);

            ViewBag.Checkout = checkout;
            ViewBag.SelectedCart = cart;
            ViewBag.ShippingOptions = _checkoutService.GetShippingOptions(checkoutId);
            ViewBag.Warnings = _checkoutService.ValidateCheckout(checkoutId);
            ViewBag.Customer = _checkoutService.LoadCustomerInfo(checkoutId);
            ViewBag.CheckoutId = checkoutId;
            ViewBag.Summary = summary;
            ViewBag.NotifyOptIn = checkout.GetNotifyOptIn();
            ViewBag.SelectedShippingOptionId = checkout.GetShippingOptionId();

            return View("~/Views/Module1/P2-6/Checkout.cshtml");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Cart");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SelectShippingOption(int checkoutId, int optionId)
    {
        try
        {
            _checkoutService.SelectShippingOption(checkoutId, optionId);
            TempData["Message"] = "Shipping option selected.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { checkoutId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateNotificationPreference(int checkoutId, bool notifyOptIn = false)
    {
        try
        {
            _checkoutService.SetOrderNotificationOptIn(checkoutId, notifyOptIn);
            TempData["Message"] = "Notification preference saved.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), new { checkoutId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmCheckout(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        try
        {
            var orderNumber = _checkoutService.ConfirmCheckout(
                checkoutId,
                nameOnCard,
                cardNumber,
                expirationDate,
                securityCode
            );

            return RedirectToAction(nameof(Success), new
            {
                checkoutId,
                orderNumber
            });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index), new { checkoutId });
        }
    }

    [HttpGet]
    public IActionResult Success(int checkoutId, string orderNumber)
    {
        ViewBag.CheckoutId = checkoutId;
        ViewBag.OrderNumber = orderNumber;
        return View("~/Views/Module1/P2-6/CheckoutSuccess.cshtml");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CancelCheckout(int checkoutId)
    {
        try
        {
            _checkoutService.CancelCheckout(checkoutId);
            TempData["Message"] = "Checkout cancelled.";
            return RedirectToAction("Index", "Cart");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index), new { checkoutId });
        }
    }
}