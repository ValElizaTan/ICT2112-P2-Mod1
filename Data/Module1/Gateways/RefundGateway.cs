using Microsoft.EntityFrameworkCore;
using ProRental.Data.Module1.Interfaces;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Gateways;

public class RefundGateway : IRefundGateway
{
    private readonly AppDbContext _context;

    public RefundGateway(AppDbContext context)
    {
        _context = context;
    }

    public List<Refund> GetRefundsByCustomerId(int customerId)
    {
        return _context.Refunds
            .Where(r => EF.Property<int>(r, "Customerid") == customerId)
            .Include(r => r.Order)
            .Include(r => r.Returnrequest)
            .ToList();
    }

    public List<Refund> GetRefundsByOrderId(int orderId)
    {
        return _context.Refunds
            .Where(r => EF.Property<int>(r, "Orderid") == orderId)
            .Include(r => r.Order)
            .Include(r => r.Returnrequest)
            .ToList();
    }

    public Refund? GetRefundByOrderId(int orderId)
    {
        return _context.Refunds
            .Where(r => EF.Property<int>(r, "Orderid") == orderId)
            .Include(r => r.Order)
            .Include(r => r.Returnrequest)
            .FirstOrDefault();
    }

    public void InsertReturnRequest(Returnrequest returnRequest)
    {
        _context.Returnrequests.Add(returnRequest);
        _context.SaveChanges();
    }

    public void InsertRefund(Refund refund)
    {
        _context.Refunds.Add(refund);
        _context.SaveChanges();
    }
}
