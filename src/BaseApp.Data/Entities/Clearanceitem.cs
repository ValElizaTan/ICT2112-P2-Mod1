using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Clearanceitem
{
    public int Clearanceitemid { get; set; }

    public int Clearancebatchid { get; set; }

    public int Inventoryitemid { get; set; }

    public decimal? Finalprice { get; set; }

    public decimal? Recommendedprice { get; set; }

    public DateTime? Saledate { get; set; }

    public virtual Clearancebatch Clearancebatch { get; set; } = null!;

    public virtual Inventoryitem Inventoryitem { get; set; } = null!;
}
