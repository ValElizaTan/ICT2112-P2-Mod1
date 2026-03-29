namespace ProRental.Data;
using ProRental.Domain.Enums;
public class TransactionResponse
{
    public int? transactionId { get; set; }
    public string providerTransactionId { get; set; }
    public string providerName { get; set; } = string.Empty;
    public TransactionStatus status { get; set; }
    public string message { get; set; }

    public TransactionResponse(int? transactionId, string providerTransactionId, TransactionStatus status, string message)
    {
        this.transactionId = transactionId;
        this.providerTransactionId = providerTransactionId;
        this.status = status;
        this.message = message;
    }

    
}
