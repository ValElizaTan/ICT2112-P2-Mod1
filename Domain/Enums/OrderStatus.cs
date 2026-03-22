namespace ProRental.Domain.Enums;

public enum OrderStatus
{
    PLACED = 0,
    PACKING = 1,
    DISPATCHED = 2,
    DELIVERED = 3,
    IN_RENTAL = 4,
    CANCELLED = 5,
    RETURN_PICKUP = 6,
    RETURNED = 7,
    INSPECTION = 8,
    REFUND_PROCESSING = 9,
    COMPLETED = 10
}
