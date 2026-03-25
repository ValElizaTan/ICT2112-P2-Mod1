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
            var shippingOptions = _checkoutService.GetShippingOptions(checkoutId);
            var warnings = _checkoutService.ValidateCheckout(checkoutId);
            var customer = _checkoutService.LoadCustomerInfo(checkoutId);

            ViewBag.Checkout = checkout;
            ViewBag.SelectedCart = cart;
            ViewBag.Summary = summary;
            ViewBag.ShippingOptions = shippingOptions;
            ViewBag.Warnings = warnings;
            ViewBag.Customer = customer;
            ViewBag.CheckoutId = checkoutId;

            int? selectedShippingOptionId = null;

            if (TempData.Peek("SelectedShippingOptionId") != null)
            {
                if (int.TryParse(TempData.Peek("SelectedShippingOptionId")?.ToString(), out var tempOptionId))
                {
                    selectedShippingOptionId = tempOptionId;
                }
            }
            else
            {
                try
                {
                    selectedShippingOptionId = checkout?.GetShippingOptionId();
                }
                catch
                {
                    selectedShippingOptionId = null;
                }
            }

            ViewBag.SelectedShippingOptionId = selectedShippingOptionId;

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
            TempData["SelectedShippingOptionId"] = optionId;
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

    // =========================
    // REAL FUTURE PAYMENT FLOW
    // Uncomment when payment feature is ready
    // =========================
    /*
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmCheckoutWithRealPayment(
        int checkoutId,
        string nameOnCard,
        string cardNumber,
        string expirationDate,
        string securityCode)
    {
        try
        {
            var details = new CreditCardPaymentDetails(
                cardNumber,
                DateOnly.Parse(expirationDate),
                int.Parse(securityCode),
                nameOnCard
            );

            var orderNumber = _checkoutService.ConfirmCheckout(checkoutId, details);

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
    */
}