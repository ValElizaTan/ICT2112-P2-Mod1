using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Orderstatushistory
{
    // ── Public accessors ─────────────────────────────────────────────────
    public int HistoryId            => Historyid;
    public int OrderId              => Orderid;
    public DateTime OccurredAt      => _timestamp;   // Timestamp is taken by scaffolded private
    public string UpdatedBy         => Updatedby;    // Updatedby (lowercase b) ≠ UpdatedBy
    public string? StatusRemark     => _remark;      // Remark is taken by scaffolded private

    // ── Factory method ────────────────────────────────────────────────────
    public static Orderstatushistory Create(int orderId, OrderHistoryStatus status, string updatedBy, string? remark)
    {
        var history = new Orderstatushistory();
        typeof(Orderstatushistory).GetProperty("Orderid",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(history, orderId);
        typeof(Orderstatushistory).GetProperty("Updatedby",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(history, updatedBy);
        typeof(Orderstatushistory).GetProperty("Timestamp",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(history, DateTime.UtcNow);
        typeof(Orderstatushistory).GetProperty("Remark",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(history, remark);
        history._status = (OrderStatus)status;
        return history;
    }
}