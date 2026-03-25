namespace ProRental.Data;
using ProRental.Domain.Enums;
public class TransactionResponse
{
    private int transactionId { get; set; }
    private string providerTransactionId { get; set; }
    private TransactionStatus status { get; set; }
    private string message { get; set; }

    public TransactionResponse(int transactionId, string providerTransactionId, TransactionStatus status, string message)
    {
        this.transactionId = transactionId;
        this.providerTransactionId = providerTransactionId;
        this.status = status;
        this.message = message;
    }

    public int TransactionId => transactionId;
    public string ProviderTransactionId => providerTransactionId;
    public TransactionStatus Status => status;
    public string Message => message;

    public string ProviderName { get; set; } = string.Empty;
}
