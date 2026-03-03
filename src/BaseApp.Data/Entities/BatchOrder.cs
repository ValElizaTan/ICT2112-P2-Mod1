using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class BatchOrder
{
    public int BatchId { get; set; }

    public int OrderId { get; set; }

    public DateTime? AddedTimestamp { get; set; }

    public virtual DeliveryBatch Batch { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
