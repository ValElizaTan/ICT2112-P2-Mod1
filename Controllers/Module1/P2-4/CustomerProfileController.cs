using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Controls;

namespace ProRental.Controllers.Module1.P24;

public class CustomerProfileController : Controller
{
    private readonly CustomerControl _control;

    public CustomerProfileController(CustomerControl control)
    {
        _control = control;
    }

    [HttpGet]
    public IActionResult Index(int customerId = 1)
    {
        return ViewProfile(customerId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCustomerDetails(int customerId, string name, string email, int phoneCountry, int phoneNumber, string passwordHash, string address, int customerType)
    {
        try
        {
            // Only update password if provided
            var finalPasswordHash = string.IsNullOrEmpty(passwordHash) ? null : passwordHash;

            _control.UpdateCustomerDetails(
                customerId,
                name,
                email,
                phoneCountry,
                phoneNumber,
                finalPasswordHash ?? "",
                address,
                customerType);

            TempData["SuccessMessage"] = "Profile updated successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating profile: {ex.Message}";
        }

        return RedirectToAction("Index", new { customerId });
    }

    [HttpGet]
    public IActionResult ViewProfile(int customerId)
    {
        try
        {
            var customer = _control.GetCustomerInformation(customerId);
            var customerInfo = customer?.GetCustomerInfo();

            // Pass customer data to view
            ViewData["CustomerInfo"] = new
            {
                CustomerId = customerInfo?.CustomerId ?? customerId,
                CustomerType = customerInfo?.CustomerType ?? 1,
                Address = customerInfo?.Address ?? "",
                Name = customerInfo?.User.Name ?? "Customer",
                Email = customerInfo?.User.Email ?? "",
                PhoneCountry = customerInfo?.User.PhoneCountry ?? 65,
                PhoneNumber = customerInfo?.User.PhoneNumber ?? "",
                Role = "Customer"
            };

            // Placeholder data for activity summary
            ViewData["OrdersPlaced"] = 0;
            ViewData["ActiveRentals"] = 0;
            ViewData["CompletedReturns"] = 0;
            ViewData["PendingRefunds"] = 0;

            // Placeholder recent activities
            ViewData["RecentActivities"] = new List<dynamic>();
        }
        catch (Exception ex)
        {
            // Fallback to placeholder data if customer not found
            ViewData["CustomerInfo"] = new
            {
                CustomerId = customerId,
                CustomerType = 1,
                Address = "123 Customer Street, Singapore",
                Name = "Customer Name",
                Email = "customer@example.com",
                PhoneCountry = 65,
                PhoneNumber = "91234567",
                Role = "Customer"
            };

            ViewData["OrdersPlaced"] = 0;
            ViewData["ActiveRentals"] = 0;
            ViewData["CompletedReturns"] = 0;
            ViewData["PendingRefunds"] = 0;
            ViewData["RecentActivities"] = new List<dynamic>();

            TempData["InfoMessage"] = "Customer profile loaded with placeholder data. Please update with real data when available.";
        }

        return View("Index");
    }
}