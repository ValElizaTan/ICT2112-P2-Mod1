using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class OrderMapper : IOrderMapper
{
    private readonly AppDbContext _db;

    public OrderMapper(AppDbContext db)
    {
        _db = db;
    }

    public void Insert(Order order)
    {
        
        _db.Orders.Add(order);
        try
        {
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"Error inserting order: {ex.Message}");
            throw; // Re-throw the exception after logging
        }
    }

    public void InsertHistory(Orderstatushistory history)
    {
        _db.Orderstatushistories.Add(history);
        _db.SaveChanges();
    }

    public void Update(Order order)
    {
        _db.Orders.Update(order);
        _db.SaveChanges();
    }

    public Order? FindById(int orderId)
    {
        return _db.Orders
            .Include(o => o.Orderitems)
            .Include(o => o.Orderstatushistories)
            .Include(o => o.Deposits)
            .AsEnumerable()
            .FirstOrDefault(o => o.OrderId == orderId);
    }

    public List<Order> FindAll()
    {
        return _db.Orders
            .Include(o => o.Orderitems)
            .AsEnumerable()
            .ToList();
    }

    public List<Order> FindByCustomer(int customerId)
    {
        return _db.Orders
            .Include(o => o.Orderitems)
            .Include(o => o.Orderstatushistories)
            .AsEnumerable()
            .Where(o => o.CustomerId == customerId)
            .ToList();
    }

    public void Delete(int orderId)
    {
        var order = _db.Orders
            .AsEnumerable()
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order != null)
        {
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }
    }
}