namespace ProRental.Domain.Module1.P24.Interfaces;

public interface IShippingService
{
    bool CreateShipment(int trackingId, double weight, string destinationAddress, bool dispatchStatus, int batchId);
    decimal CalculateShippingCost(double weight, string destinationAddress);
    void CancelShipment(int trackingId);
}
