using NpgsqlTypes;
namespace ProRental.Domain.Enums;

public enum DeliveryDuration
{
    [PgName("NextDay")]
    NextDay,

    [PgName("ThreeDays")]
    ThreeDays,

    [PgName("OneWeek")]
    OneWeek
}
