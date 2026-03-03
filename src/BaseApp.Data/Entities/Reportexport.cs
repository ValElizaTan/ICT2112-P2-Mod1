using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Reportexport
{
    public int Reportid { get; set; }

    public int? Refanalyticsid { get; set; }

    public string? Title { get; set; }

    public string? Url { get; set; }

    public virtual Analytic? Refanalytics { get; set; }
}
