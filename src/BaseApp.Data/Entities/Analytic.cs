using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Analytic
{
    public int Analyticsid { get; set; }

    public DateTime? Startdate { get; set; }

    public DateTime? Enddate { get; set; }

    public int? Loanamt { get; set; }

    public int? Returnamt { get; set; }

    public int? Primarysupplierid { get; set; }

    public int? Primaryitemid { get; set; }

    public decimal? Supplierreliability { get; set; }

    public decimal? Turnoverrate { get; set; }

    public virtual Product? Primaryitem { get; set; }

    public virtual Supplier? Primarysupplier { get; set; }

    public virtual ICollection<Reportexport> Reportexports { get; set; } = new List<Reportexport>();

    public virtual ICollection<Rentalorderlog> Transactionlogs { get; set; } = new List<Rentalorderlog>();
}
