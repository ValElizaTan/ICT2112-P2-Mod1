using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Transaction
{
    private TransactionType _type;
    private TransactionPurpose _purpose;
    private TransactionStatus _status;

    public static Transaction Create(
        int transactionId,
        decimal amount,
        TransactionType type,
        TransactionPurpose purpose,
        string? providerTransactionId)
    {
        var transaction = new Transaction
        {
            _type = type,
            _purpose = purpose,
            _status = TransactionStatus.PENDING,
            Transactionid = transactionId,
            Amount = amount,
            Providertransactionid = providerTransactionId,
            Createdat = DateTime.UtcNow
        };

        return transaction;
    }

    public void Initiate()
    {
        _status = TransactionStatus.PENDING;
        // Logic to initiate the transaction, e.g., call payment gateway API
    }

    public void MarkSuccessful()
    {
        _status = TransactionStatus.COMPLETED;
        // Additional logic for successful transaction
    }

    public void MarkFailed()
    {
        _status = TransactionStatus.FAILED;
        // Additional logic for failed transaction
    }

    public void Cancel()
    {
        _status = TransactionStatus.CANCELLED;
        // Additional logic for cancelled transaction
    }
}
