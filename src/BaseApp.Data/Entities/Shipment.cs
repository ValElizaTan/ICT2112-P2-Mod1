using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Shipment
{
    public int Trackingid { get; set; }

    public int Orderid { get; set; }

    public int Batchid { get; set; }

    public double Weight { get; set; }

    /// <summary>Maps to the PostgreSQL <c>shipment_status_enum</c> enum column. Values: PENDING, IN_TRANSIT, DELIVERED, CANCELLED.</summary>
    public string? Status { get; set; }

    public string Destination { get; set; } = null!;

    public virtual DeliveryBatch Batch { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
