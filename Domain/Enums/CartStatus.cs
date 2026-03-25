using NpgsqlTypes;
namespace ProRental.Domain.Enums;

public enum CartStatus
{
    [PgName("ACTIVE")]
    ACTIVE,

    [PgName("CHECKED_OUT")]
    CHECKED_OUT,

    [PgName("EXPIRED")]
    EXPIRED
}
