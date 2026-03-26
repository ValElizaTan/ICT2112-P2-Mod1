using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Transaction
{
    private TransactionType _type;
    private TransactionPurpose _purpose;
    private TransactionStatus _status;

    public int TransactionId => Transactionid;

    public static Transaction Create(
        decimal amount,
        TransactionType type,
        TransactionPurpose purpose,
        string? providerTransactionId,
        TransactionStatus status)
    {
        var transaction = new Transaction
        {
            _type = type,
            _purpose = purpose,
            _status = status,
            Amount = amount,
            Providertransactionid = providerTransactionId,
            Createdat = DateTime.UtcNow
        };

        return transaction;
    }

    public void ApplyProviderResult(string? providerTransactionId, TransactionStatus status)
    {
        Providertransactionid = providerTransactionId;
        _status = status;
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
