using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Loanlog
{
    public int Loanlogid { get; set; }

    public int? Orderid { get; set; }

    public DateTime? Loandate { get; set; }

    public DateTime? Returndate { get; set; }

    public DateTime? Duedate { get; set; }

    public string? Detailsjson { get; set; }
}
