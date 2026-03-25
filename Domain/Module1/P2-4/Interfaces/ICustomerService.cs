using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

// Interface contract for customer operations
public interface ICustomerService
{
    bool CreateCustomer(int customerId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash, string address, int customerType);
    void UpdateCustomerDetails(int customerId, string name, string email, int phoneCountry,
        int phoneNumber, string passwordHash, string address, int customerType);
    Customer GetCustomerInformation(int customerId);
    List<Customer> GetCustomers();
}
