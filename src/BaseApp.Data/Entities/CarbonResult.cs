using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class CarbonResult
{
    public int CarbonResultId { get; set; }

    public double? TotalCarbonKg { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? ValidationPassed { get; set; }

    public virtual ICollection<LegCarbon> LegCarbons { get; set; } = new List<LegCarbon>();
}
