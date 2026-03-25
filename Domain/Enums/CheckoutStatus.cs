using NpgsqlTypes;
namespace ProRental.Domain.Enums;

public enum CheckoutStatus
{
    [PgName("IN_PROGRESS")]
    IN_PROGRESS,

    [PgName("CONFIRMED")]
    CONFIRMED,

    [PgName("CANCELLED")]
    CANCELLED
}
