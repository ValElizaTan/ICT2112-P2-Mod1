using ProRental.Domain.Entities;

namespace ProRental.Interfaces.Data;

public interface ITransactionMapper
{
    int Insert(Transaction transaction);
    void Update(Transaction transaction);
}
