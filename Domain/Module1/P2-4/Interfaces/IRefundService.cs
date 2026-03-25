using ProRental.Domain.Entities;

namespace ProRental.Domain.Module1.P24.Interfaces;

public interface IRefundService
{
    List<Refund> GetCustomerRefunds(int customerId);
}
