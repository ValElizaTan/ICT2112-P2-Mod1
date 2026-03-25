using Microsoft.AspNetCore.Mvc;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Domain;

namespace ProRental.Domain.Services;

public class FakeShippingService : IShippingOptionService
{
    private readonly AppDbContext _context;

    public FakeShippingService(AppDbContext context)
    {
        _context = context;
    }

    public string Name => "Fake Shipping";

    public List<ShippingOption> GetShippingOptions(string orderId)
    {
        return _context.ShippingOptions
            .ToList()
            .OrderBy(x => x.GetOptionId())
            .ToList();
    }

    public void ApplyCustomerSelection(string orderId, string optionId, string preference)
    {
        // no-op for now
    }

    public List<ShippingOption> BuildOptionSet(Order order)
    {
        return GetShippingOptions(order?.OrderId.ToString() ?? "");
    }

    public IActionResult SelectShippingOption(string orderId, string optionId)
    {
        if (!int.TryParse(optionId, out var parsedOptionId))
        {
            return new BadRequestObjectResult("Invalid shipping option id.");
        }

        var exists = _context.ShippingOptions
            .ToList()
            .Any(x => x.GetOptionId() == parsedOptionId);

        if (!exists)
        {
            return new NotFoundObjectResult($"Shipping option '{optionId}' was not found.");
        }

        return new OkResult();
    }

    public IActionResult CompareOptions(string orderId)
    {
        return new OkObjectResult(GetShippingOptions(orderId));
    }

    public decimal CalculateCost(decimal subtotal, int rentalDays)
    {
        return 0m;
    }

    public bool IsAvailable(int cartId)
    {
        return true;
    }
}