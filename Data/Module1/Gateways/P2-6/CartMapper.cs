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
        var cart = _context.Set<Cart>()
            .Include(c => c.Cartitems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefault(c => EF.Property<int>(c, "Cartid") == cartId);

        HydrateCartProducts(cart);
        return cart;
    }

    public Cart? FindActiveByCustomerId(int customerId)
    {
        var cart = _context.Set<Cart>()
            .Include(c => c.Cartitems)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Productdetail)
            .FirstOrDefault(c =>
                EF.Property<int?>(c, "Customerid") == customerId &&
                EF.Property<CartStatus>(c, "Status") == CartStatus.ACTIVE);

        HydrateCartProducts(cart);
        return cart;
    }

    public Cart? FindActiveBySessionId(int sessionId)
    {
        var cart = _context.Set<Cart>()
            .Include(c => c.Cartitems)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p.Productdetail)
            .FirstOrDefault(c =>
                EF.Property<int?>(c, "Sessionid") == sessionId &&
                EF.Property<CartStatus>(c, "Status") == CartStatus.ACTIVE);

        HydrateCartProducts(cart);
        return cart;
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

    private static void HydrateCartProducts(Cart? cart)
    {
        if (cart == null) return;

        foreach (var item in cart.GetItems())
        {
            if (item.Product != null)
            {
                item.SetProduct(item.Product);
            }
        }
    }
}