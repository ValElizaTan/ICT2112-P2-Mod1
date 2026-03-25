using Microsoft.EntityFrameworkCore;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;

namespace ProRental.Data.Module1.Gateways;

public class CheckoutMapper : ICheckoutMapper
{
    private readonly AppDbContext _context;

    public CheckoutMapper(AppDbContext context)
    {
        _context = context;
    }

    public Checkout? FindById(int checkoutId)
    {
        return _context.Set<Checkout>()
            .Include(x => x.Cart)
            .Include(x => x.Customer)
            //.Include(x => x.Option)
            .FirstOrDefault(x => EF.Property<int>(x, "Checkoutid") == checkoutId);
    }

    public Checkout? FindActiveByCustomerId(int customerId)
    {
        return _context.Set<Checkout>()
            .Include(x => x.Cart)
            .Include(x => x.Customer)
            //.Include(x => x.Option)
            .FirstOrDefault(x =>
                EF.Property<int>(x, "Customerid") == customerId &&
                EF.Property<CheckoutStatus>(x, "Status") == CheckoutStatus.IN_PROGRESS);
    }

    public void Insert(Checkout checkout)
    {
        _context.Set<Checkout>().Add(checkout);
        _context.SaveChanges();
    }

    public void Update(Checkout checkout)
    {
        _context.Set<Checkout>().Update(checkout);
        _context.SaveChanges();
    }

    public void Delete(int checkoutId)
    {
        var checkout = _context.Set<Checkout>()
            .FirstOrDefault(x => EF.Property<int>(x, "Checkoutid") == checkoutId);

        if (checkout == null) return;

        _context.Set<Checkout>().Remove(checkout);
        _context.SaveChanges();
    }
}