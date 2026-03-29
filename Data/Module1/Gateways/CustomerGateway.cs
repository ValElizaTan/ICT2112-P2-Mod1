using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Gateways;

public class CustomerGateway : ICustomerGateway
{
    private readonly AppDbContext _context;

    public CustomerGateway(AppDbContext context)
    {
        _context = context;
    }

    public List<Customer> FindAll()
    {
        return _context.Customers
            .Include(c => c.User)
            .ToList();
    }

    public Customer? FindById(int customerId)
    {
        var customer = _context.Customers.Find(customerId);

        if (customer != null)
        {
            _context.Entry(customer).Reference(c => c.User).Load();
        }

        return customer;
    }

    public Customer? FindByEmail(string email)
    {
        return _context.Customers
            .Include(c => c.User)
            .AsEnumerable()
            .FirstOrDefault(c =>
                c.User != null &&
                c.User.GetUserInfo() != null &&
                c.User.GetUserInfo().Email == email);
    }

    public void InsertCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void DeleteCustomer(int customerId)
    {
        var customer = _context.Customers.Find(customerId);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}
