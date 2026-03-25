using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Controls;

public class CheckoutShippingControl
{
    private readonly IShippingOptionService _shippingOptionService;
    private readonly ICheckoutMapper _checkoutMapper;

    public CheckoutShippingControl(
        IShippingOptionService shippingOptionService,
        ICheckoutMapper checkoutMapper)
    {
        _shippingOptionService = shippingOptionService;
        _checkoutMapper = checkoutMapper;
    }

    public List<ShippingOption> GetShippingOptions(int checkoutId)
    {
        string orderId = checkoutId.ToString();
        return _shippingOptionService.GetShippingOptions(orderId);
    }

    public void SelectShippingOption(int checkoutId, int optionId)
    {
        string orderId = checkoutId.ToString();

        var result = _shippingOptionService.SelectShippingOption(orderId, optionId.ToString());

        if (result is BadRequestObjectResult badRequest)
        {
            throw new InvalidOperationException(badRequest.Value?.ToString() ?? "Invalid shipping option.");
        }

        if (result is NotFoundObjectResult notFound)
        {
            throw new InvalidOperationException(notFound.Value?.ToString() ?? "Shipping option not found.");
        }

        var checkout = _checkoutMapper.FindById(checkoutId)
            ?? throw new InvalidOperationException($"Checkout {checkoutId} was not found.");

        checkout.SetShippingOption(optionId);
        _checkoutMapper.Update(checkout);
    }

    public ShippingOption? GetSelectedShippingOption(int checkoutId)
    {
        var checkout = _checkoutMapper.FindById(checkoutId);

        if (checkout == null || !checkout.GetShippingOptionId().HasValue)
        {
            return null;
        }

        int selectedOptionId = checkout.GetShippingOptionId()!.Value;

        return GetShippingOptions(checkoutId)
            .FirstOrDefault(x => x.GetOptionId() == selectedOptionId);
    }
}