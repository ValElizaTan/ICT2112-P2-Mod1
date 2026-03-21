using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class CustomerControl : ICustomerService
{
    private readonly ICustomerGateway _customerGateway;

    public CustomerControl(ICustomerGateway customerGateway)
    {
        _customerGateway = customerGateway;
    }

    public bool CreateCustomer(int customerId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash, string address, int customerType)
    {
        var existingCustomer = _customerGateway.FindById(customerId);
        if (existingCustomer != null)
            return false;

        var user = new User(customerId, UserRole.CUSTOMER, name, email, passwordHash, phoneCountry, phoneNumber.ToString());
        var customer = new Customer(customerId, address, customerType, user);

        _customerGateway.InsertCustomer(customer);
        return true;
    }

    public void DeleteCustomer(int customerId)
    {
        _customerGateway.DeleteCustomer(customerId);
    }

    public void UpdateCustomerDetails(int customerId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash, string address, int customerType)
    {
        var customer = _customerGateway.FindById(customerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {customerId} not found.");

        var updatedUserInfo = new UserInfo(
            customer.GetCustomerInfo().User.UserId,
            customer.GetCustomerInfo().User.UserRole,
            name,
            email,
            phoneCountry,
            phoneNumber.ToString()
        );

        var updatedCustomerInfo = new CustomerInfo(
            customerId,
            address,
            customerType,
            updatedUserInfo
        );

        customer.SetCustomerInfo(updatedCustomerInfo);
        _customerGateway.UpdateCustomer(customer);
    }

    public Customer GetCustomerInformation(int customerId)
    {
        var customer = _customerGateway.FindById(customerId);
        if (customer == null)
            throw new InvalidOperationException($"Customer with ID {customerId} not found.");
        return customer;
    }

    public List<Customer> GetCustomers()
    {
        // Will be expanded with ICustomerGateway.FindAll() when needed
        return new List<Customer>();
    }
}
