using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module6.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Domain;

namespace ProRental.Controllers.Module1;

public class CheckoutController : Controller
{
    private readonly ICartService   _cartService;
    private readonly IOrderService  _orderService;

    public CheckoutController(ICartService cartService, IOrderService orderService)
    {
        _cartService  = cartService;
        _orderService = orderService;
    }

    // GET: /Checkout
    public IActionResult Index()
    {
        var cart   = _cartService.GetOrCreateActiveCartBySession("test-session");
        var cartId = cart.GetCartId();

        if (cartId == null)
            return RedirectToAction("Index", "Cart");

        // Redirect back if nothing selected
        if (!_cartService.CanProceedToCheckout(cartId))
        {
            TempData["Error"] = "Please select at least one item before checking out.";
            return RedirectToAction("Index", "Cart");
        }

        var draft = _cartService.BuildSelectedOrderDraft(cartId);
        var costs = _cartService.GetCartCosts(cartId);

        var vm = new CheckoutViewModel
        {
            Draft       = draft,
            Costs       = costs,
            StartDate   = draft.StartDate ?? DateTime.Today.AddDays(1),
            EndDate     = draft.EndDate   ?? DateTime.Today.AddDays(2),
        };

        return View("~/Views/Module1/P2-6/Checkout.cshtml", vm);
    }

    // POST: /Checkout/PlaceOrder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(DateTime startDate, DateTime endDate,
                                    DeliveryDuration deliveryType, bool notifyOptIn = false)
    {
        var cart   = _cartService.GetOrCreateActiveCartBySession("test-session");
        var cartId = cart.GetCartId();

        if (cartId == null || !_cartService.CanProceedToCheckout(cartId))
        {
            TempData["Error"] = "Your session expired. Please try again.";
            return RedirectToAction("Index", "Cart");
        }

        // Set rental period on cart
        _cartService.SetRentalPeriod(cartId, startDate, endDate);

        var costs = _cartService.GetCartCosts(cartId);
        var draft = _cartService.BuildSelectedOrderDraft(cartId);

        // Build item tuples for IOrderService
        var itemData = draft.Items.Select(item =>
        (
            productId   : item.GetProductId(),
            quantity    : item.GetQuantity(),
            unitPrice   : item.GetProduct()?.GetPrice() ?? 0m,
            rentalStart : startDate,
            rentalEnd   : endDate
        )).ToList();

        var productQuantities = draft.Items
            .ToDictionary(i => i.GetProductId(), i => i.GetQuantity());

        // Use 0 for customerId/checkoutId until auth is wired up
        var order = _orderService.CreateOrder(
            customerId        : 0,
            checkoutId        : 0,
            itemData          : itemData,
            deliveryType      : deliveryType,
            totalAmount       : costs.RentalCost + costs.DeliveryCost,
            productQuantities : productQuantities
        );

        // Clear cart after successful order
        _cartService.EmptyCart(cartId);

        TempData["OrderId"] = order.OrderId;
        return RedirectToAction("Confirmation");
    }

    // GET: /Checkout/Confirmation
    public IActionResult Confirmation()
    {
        var orderId = TempData["OrderId"];
        if (orderId == null)
            return RedirectToAction("Index", "Catalogue");

        var order = _orderService.GetOrder((int)orderId);
        return View("~/Views/Module1/P2-6/OrderConfirmation.cshtml", order);
    }
}