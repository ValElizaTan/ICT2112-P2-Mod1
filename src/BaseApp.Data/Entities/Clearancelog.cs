using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Clearancelog
{
    public int Clearancelogid { get; set; }

    public string? Batchname { get; set; }

    public int? Clearanceitemid { get; set; }

    public DateTime? Clearancedate { get; set; }

    public decimal? Finalprice { get; set; }

    public decimal? Recommendedprice { get; set; }

    public DateTime? Saledate { get; set; }

    public string? Detailsjson { get; set; }
}
