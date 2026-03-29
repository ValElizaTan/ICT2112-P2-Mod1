using System.Reflection;
using ProRental.Data.UnitOfWork;
using ProRental.Domain.Entities;
using ProRental.Interfaces.Data;

namespace ProRental.Data;

public class TransactionMapper : ITransactionMapper
{
    private readonly AppDbContext _db;

    public TransactionMapper(AppDbContext db)
    {
        _db = db;
    }

    public int Insert(Transaction transaction)
    {
        _db.Transactions.Add(transaction);
        _db.SaveChanges();

        var transactionIdValue = _db.Entry(transaction).Property("Transactionid").CurrentValue;
        if (transactionIdValue != null)
        {
            typeof(Transaction)
                .GetField("_transactionid", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(transaction, transactionIdValue);
        }

        return transaction.TransactionId;
    }

    public void Update(Transaction transaction)
    {
        _db.Transactions.Update(transaction);
        _db.SaveChanges();
    }
}
