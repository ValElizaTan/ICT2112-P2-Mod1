using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Rentalorderlog
{
    public int Rentalorderid { get; set; }

    public int? Orderid { get; set; }

    public int? Customerid { get; set; }

    public DateTime? Orderdate { get; set; }

    public decimal? Totalamount { get; set; }

    public string? Detailsjson { get; set; }

    public virtual ICollection<Analytic> Analytics { get; set; } = new List<Analytic>();
}
