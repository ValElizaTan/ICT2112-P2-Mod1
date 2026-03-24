using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module1;

public class Module1Controller : Controller
{
    private readonly ICustomerGateway _customerGateway;

    public Module1Controller(ICustomerGateway customerGateway)
    {
        _customerGateway = customerGateway;
    }

    // GET /Module1/Login
    public IActionResult Login() => View("Login/Login");

    // POST /Module1/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string email, string password, bool rememberMe = false)
    {
        // keep your current temporary login flow
        var customer = _customerGateway.FindByEmail(email);

        if (customer == null)
        {
            ViewData["ErrorMessage"] = "Invalid email or password.";
            return View("Login/Login");
        }

        HttpContext.Session.SetString("UserEmail", email);
        HttpContext.Session.SetInt32("CustomerId", GetCustomerIdForSession(customer));

        return RedirectToAction("Index", "Home");
    }

    private static int GetCustomerIdForSession(Customer customer)
    {
        var prop = customer.GetType().GetProperty("Customerid",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic);

        if (prop?.GetValue(customer) is int id)
            return id;

        var field = customer.GetType().GetField("_customerid",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic);

        if (field?.GetValue(customer) is int fieldId)
            return fieldId;

        throw new InvalidOperationException("Unable to read CustomerId from Customer entity.");
    }

    // GET /Module1/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    // GET /Module1/Signup
    public IActionResult Signup() => View("Login/Signup");

    // POST /Module1/Signup
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Signup(string firstName, string lastName, string email,
                               string? phone, string password, string confirmPassword,
                               bool agreeTerms = false)
    {
        if (!agreeTerms)
        {
            ViewData["ErrorMessage"] = "You must agree to the Terms of Service to create an account.";
            return View("Login/Signup");
        }

        if (password != confirmPassword)
        {
            ViewData["ErrorMessage"] = "Passwords do not match.";
            return View("Login/Signup");
        }

        if (password.Length < 8)
        {
            ViewData["ErrorMessage"] = "Password must be at least 8 characters.";
            return View("Login/Signup");
        }

        if (_customerGateway.FindByEmail(email) != null)
        {
            ViewData["ErrorMessage"] = "An account with that email already exists.";
            return View("Login/Signup");
        }

        // Parse phone: "+65 91234567" → country=65, number="91234567"
        int phoneCountry = 65;
        string phoneNumber = "";
        if (!string.IsNullOrWhiteSpace(phone))
        {
            var stripped = phone.TrimStart('+');
            var parts = stripped.Split(' ', 2);
            if (parts.Length == 2 && int.TryParse(parts[0], out int cc))
            {
                phoneCountry = cc;
                phoneNumber = parts[1].Replace(" ", "");
            }
            else
            {
                phoneNumber = stripped.Replace(" ", "");
            }
        }

        var passwordHash = HashPassword(password);
        var name = $"{firstName.Trim()} {lastName.Trim()}";
        var user = new User(0, UserRole.CUSTOMER, name, email, passwordHash, phoneCountry, phoneNumber);
        var customer = new Customer(0, "", 1, user);

        try
        {
            _customerGateway.InsertCustomer(customer);
            TempData["SuccessMessage"] = "Account created successfully! Please sign in.";
            return RedirectToAction("Login");
        }
        catch (Exception)
        {
            ViewData["ErrorMessage"] = "Registration failed. The email may already be in use.";
            return View("Login/Signup");
        }
    }

    // GET /Module1/StaffLogin
    public IActionResult StaffLogin() => View("P2-6/StaffLogin");

    // POST /Module1/StaffLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StaffLogin(string staffEmail, string staffPassword)
    {
        // TODO: add staff authentication logic here (Team P2-6)
        return RedirectToAction("Index", "StaffDashboard");
    }

    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}