using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Packagingprofile
{
    public int Profileid { get; set; }

    public int Orderid { get; set; }

    public double Volume { get; set; }

    public string? Fragilitylevel { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Packagingconfiguration> Packagingconfigurations { get; set; } = new List<Packagingconfiguration>();
}
