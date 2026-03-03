using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Packagingconfigmaterial
{
    public int Configurationid { get; set; }

    public int Materialid { get; set; }

    public string? Category { get; set; }

    public int Quantity { get; set; }

    public virtual Packagingconfiguration Configuration { get; set; } = null!;

    public virtual Packagingmaterial Material { get; set; } = null!;
}
