namespace ProRental.Domain.Entities;

public class ShipmentData
{
    public int TrackingId { get; set; }
    public double Weight { get; set; }
    public string DestinationAddress { get; set; } = string.Empty;
    public bool DispatchStatus { get; set; }
    public int BatchId { get; set; }
}
