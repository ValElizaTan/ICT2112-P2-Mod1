using ProRental.Domain.Entities;

namespace ProRental.Data.Module1.Interfaces;

public interface IRefundGateway
{
    List<Refund> GetRefundsByCustomerId(int customerId);
}
