using System;
using System.ComponentModel.DataAnnotations;
using ProRental.Domain.Enums;

namespace ProRental.Controllers.Module1;

public class MockCheckoutViewModel
{
    [Required]
    public int OrderId { get; set; } = 1001;

    [Required]
    [Range(1, 1000000)]
    public decimal Amount { get; set; } = 120m;

    [Required]
    public TransactionPurpose Purpose { get; set; } = TransactionPurpose.ORDER;

    [Required]
    [StringLength(80)]
    public string NameOnCard { get; set; } = string.Empty;

    [Required]
    [StringLength(19, MinimumLength = 12)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, 12)]
    public int ExpirationMonth { get; set; } = 12;

    [Required]
    [Range(2024, 2100)]
    public int ExpirationYear { get; set; } = DateTime.UtcNow.Year + 1;

    [Required]
    [Range(100, 9999)]
    public int SecurityCode { get; set; } = 123;
}
