using System;
using System.Collections.Generic;

namespace BaseApp.Data.Entities;

public partial class Route
{
    public int RouteId { get; set; }

    public string OriginAddress { get; set; } = null!;

    public string DestinationAddress { get; set; } = null!;

    public double? TotalDistanceKm { get; set; }

    public bool? IsValid { get; set; }

    /// <summary>Maps to the PostgreSQL <c>transport_mode_combination</c> enum column. Values: TRUCK_ONLY, SHIP_TRUCK, AIR_TRUCK, RAIL_TRUCK, MULTIMODAL.</summary>
    public string? ModeCombination { get; set; }

    public int? OriginHubId { get; set; }

    public int? DestinationHubId { get; set; }

    public virtual TransportationHub? DestinationHub { get; set; }

    public virtual TransportationHub? OriginHub { get; set; }

    public virtual ICollection<RouteLeg> RouteLegs { get; set; } = new List<RouteLeg>();

    public virtual ICollection<ShippingOption> ShippingOptions { get; set; } = new List<ShippingOption>();
}
