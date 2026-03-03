using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Ecobadge
{
    public int Badgeid { get; set; }

    public double Maxcarbong { get; set; }

    public string? Criteriadescription { get; set; }

    public string Badgename { get; set; } = null!;

    public virtual ICollection<Productfootprint> Productfootprints { get; set; } = new List<Productfootprint>();
}
