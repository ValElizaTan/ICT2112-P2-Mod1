using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;
using ProRental.Interfaces.Domain;
using System.Reflection;

namespace ProRental.Domain.Controls;

public class CheckoutShippingControl
{
    // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
    // private readonly IShippingOptionService _shippingOptionService;
    private readonly ICheckoutMapper _checkoutMapper;

    private static readonly Dictionary<string, int> _selectedOptionByOrderId = new();

    // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
    /*
    public CheckoutShippingControl(
        IShippingOptionService shippingOptionService,
        ICheckoutMapper checkoutMapper)
    {
        _shippingOptionService = shippingOptionService;
        _checkoutMapper = checkoutMapper;
    }
    */

    // TEMP HARDCODED LOGIC FOR TESTING NOW
    public CheckoutShippingControl(ICheckoutMapper checkoutMapper)
    {
        _checkoutMapper = checkoutMapper;
    }

    public List<ShippingOption> GetShippingOptions(int checkoutId)
    {
        string orderId = checkoutId.ToString();

        // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
        // return _shippingOptionService.GetShippingOptions(orderId);

        // TEMP HARDCODED LOGIC FOR TESTING NOW
        return BuildFakeShippingOptions();
    }

    // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
    // public IActionResult ApplyCustomerSelection(int checkoutId, int optionId, PreferenceType preference)
    public IActionResult ApplyCustomerSelection(int checkoutId, int optionId, string preference)
    {
        string orderId = checkoutId.ToString();

        // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
        // return _shippingOptionService.ApplyCustomerSelection(orderId, optionId.ToString(), preference);

        // TEMP HARDCODED LOGIC FOR TESTING NOW
        return SelectShippingOption(checkoutId, optionId);
    }

    public List<ShippingOption> BuildOptionSet(Order order)
    {
        // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
        // return _shippingOptionService.buildOptionSet(order);

        // TEMP HARDCODED LOGIC FOR TESTING NOW
        return BuildFakeShippingOptions();
    }

    // public IActionResult SelectShippingOption(int checkoutId, int optionId)
    // {
    //     string orderId = checkoutId.ToString();

    //     // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
    //     // return _shippingOptionService.SelectShippingOption(orderId, optionId.ToString());

    //     // TEMP HARDCODED LOGIC FOR TESTING NOW
    //     var options = GetShippingOptions(checkoutId);

    //     bool exists = options.Any(x => GetOptionId(x) == optionId);

    //     if (!exists)
    //     {
    //         return new BadRequestObjectResult("Invalid shipping option id.");
    //     }

    //     _selectedOptionByOrderId[orderId] = optionId;

    //     // REAL LOGIC - USE THIS WHEN CHECKOUT OPTION_ID / SHIPPINGOPTION TABLE IS READY
    //     /*
    //     var checkout = _checkoutMapper.FindById(checkoutId);
    //     if (checkout != null)
    //     {
    //         checkout.SetShippingOption(optionId);
    //         _checkoutMapper.Update(checkout);
    //     }
    //     */

    //     return new OkObjectResult($"Shipping option '{optionId}' selected.");
    // }

    public IActionResult SelectShippingOption(int checkoutId, int optionId)
    {
        string orderId = checkoutId.ToString();

        var options = GetShippingOptions(checkoutId);
        bool exists = options.Any(x => GetOptionId(x) == optionId);

        if (!exists)
        {
            return new BadRequestObjectResult("Invalid shipping option id.");
        }

        _selectedOptionByOrderId[orderId] = optionId;

        // TEMP ONLY:
        // Do not persist fake hardcoded option ids into Checkout.OptionId yet,
        // because these ids do not exist in the real ShippingOption table.
        // var checkout = _checkoutMapper.FindById(checkoutId);
        // if (checkout != null)
        // {
        //     checkout.SetShippingOption(optionId);
        //     _checkoutMapper.Update(checkout);
        // }

        return new OkObjectResult($"Shipping option '{optionId}' selected.");
    }

    public IActionResult CompareOptions(int checkoutId)
    {
        string orderId = checkoutId.ToString();

        // REAL LOGIC - USE THIS WHEN IShippingOptionService IS READY
        // return _shippingOptionService.CompareOptions(orderId);

        // TEMP HARDCODED LOGIC FOR TESTING NOW
        var options = GetShippingOptions(checkoutId);

        var result = options.Select(x => new
        {
            OptionId = GetOptionId(x),
            DisplayName = GetDisplayName(x),
            Cost = GetCost(x),
            DeliveryDays = GetDeliveryDays(x)
        }).ToList();

        return new OkObjectResult(result);
    }

    public ShippingOption? GetSelectedShippingOption(int checkoutId)
    {
        string orderId = checkoutId.ToString();
        var options = GetShippingOptions(checkoutId);

        if (!_selectedOptionByOrderId.TryGetValue(orderId, out var selectedOptionId))
        {
            return null;
        }

        return options.FirstOrDefault(x => GetOptionId(x) == selectedOptionId);
    }

    private List<ShippingOption> BuildFakeShippingOptions()
    {
        return new List<ShippingOption>
        {
            CreateShippingOption(1, "Standard Delivery", 5.90m, 3),
            CreateShippingOption(2, "Express Delivery", 12.90m, 1),
            CreateShippingOption(3, "Eco Delivery", 4.50m, 7)
        };
    }

    private ShippingOption CreateShippingOption(
        int optionId,
        string displayName,
        decimal cost,
        int deliveryDays)
    {
        var option = (ShippingOption)Activator.CreateInstance(typeof(ShippingOption), nonPublic: true)!;

        SetMemberValue(option, optionId, "_optionId", "optionId");
        SetMemberValue(option, displayName, "_displayName", "displayName");
        SetMemberValue(option, cost, "_cost", "cost");
        SetMemberValue(option, deliveryDays, "_deliveryDays", "deliveryDays");

        return option;
    }

    private int GetOptionId(ShippingOption option)
    {
        var value = GetMemberValue(option, "getOptionId", "_optionId", "optionId");

        if (int.TryParse(value, out var result))
        {
            return result;
        }

        return 0;
    }

    private string? GetDisplayName(ShippingOption option)
    {
        return GetMemberValue(option, "getDisplayName", "_displayName", "displayName");
    }

    private decimal? GetCost(ShippingOption option)
    {
        var value = GetMemberValue(option, "getCost", "_cost", "cost");

        if (decimal.TryParse(value, out var result))
        {
            return result;
        }

        return null;
    }

    private int? GetDeliveryDays(ShippingOption option)
    {
        var value = GetMemberValue(option, "getDeliveryDays", "_deliveryDays", "deliveryDays");

        if (int.TryParse(value, out var result))
        {
            return result;
        }

        return null;
    }

    private string? GetMemberValue(object target, params string[] possibleNames)
    {
        var type = target.GetType();

        foreach (var name in possibleNames)
        {
            var method = type.GetMethod(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null && method.GetParameters().Length == 0)
            {
                var value = method.Invoke(target, null);
                if (value != null)
                {
                    return value.ToString();
                }
            }

            var prop = type.GetProperty(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
            {
                var value = prop.GetValue(target);
                if (value != null)
                {
                    return value.ToString();
                }
            }

            var field = type.GetField(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                var value = field.GetValue(target);
                if (value != null)
                {
                    return value.ToString();
                }
            }
        }

        return null;
    }

    private void SetMemberValue(object target, object value, params string[] possibleNames)
    {
        var type = target.GetType();

        foreach (var name in possibleNames)
        {
            string trimmed = name.TrimStart('_');
            if (!string.IsNullOrEmpty(trimmed))
            {
                string setterName = "set" + char.ToUpper(trimmed[0]) + trimmed.Substring(1);

                var method = type.GetMethod(
                    setterName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (method != null && method.GetParameters().Length == 1)
                {
                    try
                    {
                        var parameterType = method.GetParameters()[0].ParameterType;
                        var converted = ConvertValue(value, parameterType);
                        method.Invoke(target, new[] { converted });
                        return;
                    }
                    catch
                    {
                    }
                }
            }

            var prop = type.GetProperty(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                try
                {
                    var converted = ConvertValue(value, prop.PropertyType);
                    prop.SetValue(target, converted);
                    return;
                }
                catch
                {
                }
            }

            var field = type.GetField(
                name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                try
                {
                    var converted = ConvertValue(value, field.FieldType);
                    field.SetValue(target, converted);
                    return;
                }
                catch
                {
                }
            }
        }
    }

    private object? ConvertValue(object value, Type targetType)
    {
        var nullableType = Nullable.GetUnderlyingType(targetType);
        if (nullableType != null)
        {
            targetType = nullableType;
        }

        if (targetType == typeof(string))
        {
            return value.ToString();
        }

        if (targetType.IsEnum)
        {
            if (value is string str)
            {
                try
                {
                    return Enum.Parse(targetType, str, true);
                }
                catch
                {
                    return Activator.CreateInstance(targetType);
                }
            }

            return Activator.CreateInstance(targetType);
        }

        return Convert.ChangeType(value, targetType);
    }
}