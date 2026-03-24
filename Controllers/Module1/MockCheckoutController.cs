using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class MockCheckoutController : Controller
{
    private readonly IPaymentGatewayService _paymentGatewayService;

    public MockCheckoutController(IPaymentGatewayService paymentGatewayService)
    {
        _paymentGatewayService = paymentGatewayService;
    }

    [HttpGet]
    public IActionResult MockCheckout()
    {
        return View("~/Views/Module1/P2-6/MockCheckout.cshtml", new MockCheckoutViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MockCheckout(MockCheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Module1/P2-6/MockCheckout.cshtml", model);
        }

        var expirationDate = new DateOnly(model.ExpirationYear, model.ExpirationMonth, 1);
        var paymentDetails = new CreditCardPaymentDetails(
            model.CardNumber,
            expirationDate,
            model.SecurityCode,
            model.NameOnCard);

        var result = _paymentGatewayService.MakePayment(
            model.OrderId,
            model.Amount,
            model.Purpose,
            paymentDetails);

        var resultViewModel = new MockCheckoutResultViewModel
        {
            OrderId = model.OrderId,
            Amount = model.Amount,
            Purpose = model.Purpose,
            ProviderName = result.ProviderName,
            ProviderTransactionId = result.ProviderTransactionId,
            Status = result.Status,
            Message = result.Message
        };

        return View("~/Views/Module1/P2-6/MockCheckoutResult.cshtml", resultViewModel);
    }
}
