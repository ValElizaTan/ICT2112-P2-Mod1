using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface IRefundService
{
    List<Refund> GetCustomerRefunds(int customerId);
    Refund? GetRefundByOrderId(int orderId);
    bool InitiateReturn(int orderId, int customerId, decimal refundAmount, string returnMethod);
}
