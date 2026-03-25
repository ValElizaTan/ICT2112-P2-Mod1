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
}
