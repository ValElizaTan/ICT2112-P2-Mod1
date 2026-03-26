using ProRental.Domain.Enums;
using System.Reflection;

namespace ProRental.Domain.Entities;

public partial class Orderstatushistory
{
    // _status and Status property are defined in P2-4 partial (as OrderStatus)

    // ── Public accessors ─────────────────────────────────────────────────
    public int HistoryId            => Historyid;
    public int OrderId              => Orderid;
    public DateTime OccurredAt      => _timestamp;
    public string UpdatedBy         => Updatedby;
    public string? StatusRemark     => _remark;

    // ── Factory method ───────────────────────────────────────────────────
    public static Orderstatushistory Create(int orderId, OrderHistoryStatus status,
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
        history._status = (OrderStatus)status;
        return history;
    }
}
