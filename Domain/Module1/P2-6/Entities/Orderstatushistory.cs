using ProRental.Domain.Enums;
using System.Reflection;

namespace ProRental.Domain.Entities;

public partial class Orderstatushistory
{
    // ── Enum-typed field (mapped via AppDbContext.Custom.cs) ─────────────
    private OrderStatus? _status;
    private OrderStatus? Status { get => _status; set => _status = value; }

    // ── Public accessors ─────────────────────────────────────────────────
    public int HistoryId            => Historyid;
    public int OrderId              => Orderid;
    public OrderStatus? CurrentStatus => _status;
    public DateTime OccurredAt      => _timestamp;
    public string UpdatedBy         => Updatedby;
    public string? StatusRemark     => _remark;

    // ── Factory method ───────────────────────────────────────────────────
    public static Orderstatushistory Create(int orderId, OrderStatus status,
                                            string updatedBy, string? remark = null)
    {
        var history = new Orderstatushistory();
        typeof(Orderstatushistory).GetProperty("Orderid",
            BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(history, orderId);
        typeof(Orderstatushistory).GetProperty("Updatedby",
            BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(history, updatedBy);
        typeof(Orderstatushistory).GetProperty("Timestamp",
            BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(history, DateTime.UtcNow);
        typeof(Orderstatushistory).GetProperty("Remark",
            BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(history, remark);
        history._status = status;
        return history;
    }
}