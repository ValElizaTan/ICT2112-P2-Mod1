using ProRental.Domain.Enums;

namespace ProRental.Domain.Module1.P24.Entities;

/// <summary>
/// ShipmentFilter is a value object (not persisted) used for filtering shipments
/// in the staff dashboard and search operations.
/// </summary>
public class ShipmentFilter
{
    private readonly DateTime? _fromDate;
    private readonly DateTime? _toDate;
    private readonly int? _carrierId;
    private readonly ShipmentStatus? _status;
    private readonly string? _route;

    public ShipmentFilter(DateTime? fromDate = null, DateTime? toDate = null, 
                         int? carrierId = null, ShipmentStatus? status = null, 
                         string? route = null)
    {
        _fromDate = fromDate;
        _toDate = toDate;
        _carrierId = carrierId;
        _status = status;
        _route = route;
    }

    public DateTime? GetFromDate() => _fromDate;
    public DateTime? GetToDate() => _toDate;
    public int? GetCarrierId() => _carrierId;
    public ShipmentStatus? GetStatus() => _status;
    public string? GetRoute() => _route;

    public bool IsEmpty() => !_fromDate.HasValue && !_toDate.HasValue && 
                             !_carrierId.HasValue && !_status.HasValue && string.IsNullOrEmpty(_route);
}
