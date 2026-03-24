using Microsoft.EntityFrameworkCore;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Interfaces.Data;
using ProRental.Data.UnitOfWork;

namespace ProRental.Data.Module1.Gateways;

public class CartMapper : ICartMapper
{
    private readonly AppDbContext _context;

    public CartMapper(AppDbContext context)
    {
        _context = context;
    }

    public Cart? FindById(int cartId)
    {
        return _context.Set<Cart>()
            .Include(c => c.Cartitems)
            .FirstOrDefault(c => EF.Property<int>(c, "Cartid") == cartId);
    }

    public Cart? FindActiveByCustomerId(int customerId)
    {
        return _context.Set<Cart>()
            .Include(c => c.Cartitems)
            .FirstOrDefault(c =>
                EF.Property<int?>(c, "Customerid") == customerId &&
                EF.Property<CartStatus>(c, "Status") == CartStatus.ACTIVE);
    }

    public Cart? FindActiveBySessionId(int sessionId)
    {
        return _context.Set<Cart>()
            .Include(c => c.Cartitems)
            .FirstOrDefault(c =>
                EF.Property<int?>(c, "Sessionid") == sessionId &&
                EF.Property<CartStatus>(c, "Status") == CartStatus.ACTIVE);
    }
    public void Insert(Cart cart)
    {
        _context.Set<Cart>().Add(cart);
        _context.SaveChanges();
    }

    public void Update(Cart cart)
    {
        _context.Set<Cart>().Update(cart);
        _context.SaveChanges();
    }

    public void Delete(int cartId)
    {
        var cart = _context.Set<Cart>()
            .FirstOrDefault(c => EF.Property<int>(c, "Cartid") == cartId);

        if (cart != null)
        {
            _context.Set<Cart>().Remove(cart);
            _context.SaveChanges();
        }
    }
}