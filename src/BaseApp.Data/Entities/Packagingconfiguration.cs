using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Packagingconfiguration
{
    public int Configurationid { get; set; }

    public int Profileid { get; set; }

    public virtual ICollection<Packagingconfigmaterial> Packagingconfigmaterials { get; set; } = new List<Packagingconfigmaterial>();

    public virtual Packagingprofile Profile { get; set; } = null!;
}
