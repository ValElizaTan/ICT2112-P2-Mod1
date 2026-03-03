using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Returnlog
{
    public int Returnlogid { get; set; }

    public int? Customerid { get; set; }

    public int? Returnrequestid { get; set; }

    public int? Returnitemid { get; set; }

    public decimal? Refundamount { get; set; }

    public DateTime? Requestdate { get; set; }

    public DateTime? Completiondate { get; set; }

    public string? Imageurl { get; set; }

    public string? Detailsjson { get; set; }
}
