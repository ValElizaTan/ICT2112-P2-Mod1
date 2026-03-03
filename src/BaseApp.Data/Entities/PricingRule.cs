using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class PricingRule
{
    public int RuleId { get; set; }

    public decimal? BaseRatePerKm { get; set; }

    public bool? IsActive { get; set; }

    public decimal? CarbonSurcharge { get; set; }
}
