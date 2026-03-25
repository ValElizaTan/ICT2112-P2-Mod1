using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface ICustomerGateway
{
    Customer? FindById(int customerId);
    Customer? FindByEmail(string email);
    void InsertCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
}
