using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface IRefundGateway
{
    List<Refund> GetRefundsByCustomerId(int customerId);
    List<Refund> GetRefundsByOrderId(int orderId);
    Refund? GetRefundByOrderId(int orderId);
    void InsertReturnRequest(Returnrequest returnRequest);
    void InsertRefund(Refund refund);
}
