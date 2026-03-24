using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module1;

public class MockCheckoutResultViewModel
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public TransactionPurpose Purpose { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public string ProviderTransactionId { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
}
