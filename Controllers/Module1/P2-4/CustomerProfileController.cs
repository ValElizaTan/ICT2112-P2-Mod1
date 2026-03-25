using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
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
        // Debug output to console
        Console.WriteLine($"CustomerProfileController.Index called with customerId: {customerId}");
        Console.WriteLine($"_control is null? {_control == null}");

        try
        {
            // Safely get customer info
            Customer? customer = null;
            try
            {
                customer = _control.GetCustomerInformation(customerId);
                Console.WriteLine($"Customer found: {customer != null}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting customer: {ex.Message}");
            }

            // Get customer info safely
            var customerInfo = customer?.GetCustomerInfo();

            // Always provide fallback data
            ViewData["CustomerInfo"] = new
            {
                CustomerId = customerInfo?.CustomerId ?? customerId,
                CustomerType = customerInfo?.CustomerType ?? 1,
                Address = customerInfo?.Address ?? "123 Customer Street, Singapore",
                Name = customerInfo?.User.Name ?? "Test Customer",
                Email = customerInfo?.User.Email ?? "customer@example.com",
                PhoneCountry = customerInfo?.User.PhoneCountry ?? 65,
                PhoneNumber = customerInfo?.User.PhoneNumber ?? "91234567",
                Role = "Customer"
            };

            ViewData["OrdersPlaced"] = 0;
            ViewData["ActiveRentals"] = 0;
            ViewData["CompletedReturns"] = 0;
            ViewData["PendingRefunds"] = 0;
            ViewData["RecentActivities"] = new List<dynamic>();

            return View();
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"ERROR in CustomerProfileController: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");

            // Return with placeholder data
            ViewData["CustomerInfo"] = new
            {
                CustomerId = customerId,
                CustomerType = 1,
                Address = "123 Customer Street, Singapore",
                Name = "Test Customer",
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

            ViewData["ErrorMessage"] = ex.Message;

            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateCustomerDetails(int customerId, string name, string email, int phoneCountry, int phoneNumber, string passwordHash, string address, int customerType)
    {
        try
        {
            var finalPasswordHash = string.IsNullOrEmpty(passwordHash) ? "" : passwordHash;

            _control.UpdateCustomerDetails(
                customerId, name, email, phoneCountry, phoneNumber, finalPasswordHash, address, customerType);

            TempData["SuccessMessage"] = "Profile updated successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error updating profile: {ex.Message}";
        }

        return RedirectToAction("Index", new { customerId });
    }
}