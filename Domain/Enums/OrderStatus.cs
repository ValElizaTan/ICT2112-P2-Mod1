using NpgsqlTypes;

namespace ProRental.Domain.Enums;

public enum OrderStatus
{
    [PgName("PENDING")]
    PENDING = 0,

    [PgName("CONFIRMED")]
    CONFIRMED = 1,

    [PgName("PACKING")]
    PACKING = 2,

    [PgName("READY_FOR_DISPATCH")]
    READY_FOR_DISPATCH = 3,

    [PgName("DISPATCHED")]
    DISPATCHED = 4,

    [PgName("DELIVERED")]
    DELIVERED = 5,

    [PgName("IN_RENTAL")]
    IN_RENTAL = 6,

    [PgName("CANCELLED")]
    CANCELLED = 7,

    [PgName("RETURN_PICKUP")]
    RETURN_PICKUP = 8,

    [PgName("RETURNED")]
    RETURNED = 9,

    [PgName("INSPECTION")]
    INSPECTION = 10,

    [PgName("REFUND_PROCESSING")]
    REFUND_PROCESSING = 11,

    [PgName("COMPLETED")]
    COMPLETED = 12
}