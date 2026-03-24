using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Transaction
{
    private TransactionType type;
    private TransactionPurpose purpose;
    private TransactionStatus status;

    public static Transaction Create(
        int transactionId,
        decimal amount,
        TransactionType type,
        TransactionPurpose purpose,
        string? providerTransactionId)
    {
        var transaction = new Transaction
        {
            type = type,
            purpose = purpose,
            status = TransactionStatus.PENDING,
            Transactionid = transactionId,
            Amount = amount,
            Providertransactionid = providerTransactionId,
            Createdat = DateTime.UtcNow
        };

        return transaction;
    }

    public void Initiate()
    {
        status = TransactionStatus.PENDING;
        // Logic to initiate the transaction, e.g., call payment gateway API
    }

    public void MarkSuccessful()
    {
        status = TransactionStatus.COMPLETED;
        // Additional logic for successful transaction
    }

    public void MarkFailed()
    {
        status = TransactionStatus.FAILED;
        // Additional logic for failed transaction
    }

    public void Cancel()
    {
        status = TransactionStatus.CANCELLED;
        // Additional logic for cancelled transaction
    }
}
