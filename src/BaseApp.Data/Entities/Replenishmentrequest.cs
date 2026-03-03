using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Replenishmentrequest
{
    public int Requestid { get; set; }

    public int? Requestedby { get; set; }

    public DateTime? Createdat { get; set; }

    public string? Remarks { get; set; }

    public DateTime? Completedat { get; set; }

    public int? Completedby { get; set; }

    public virtual User? CompletedbyNavigation { get; set; }

    public virtual ICollection<Lineitem> Lineitems { get; set; } = new List<Lineitem>();

    public virtual User? RequestedbyNavigation { get; set; }
}
