using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Domain.Module1.P24.Controls;

public class RefundControl : IRefundService
{
    private readonly IRefundGateway _refundGateway;

    public RefundControl(IRefundGateway refundGateway)
    {
        _refundGateway = refundGateway;
    }

    public List<Refund> GetCustomerRefunds(int customerId)
    {
        try
        {
            return _refundGateway.GetRefundsByCustomerId(customerId);
        }
        catch
        {
            return new List<Refund>();
        }
    }
}
