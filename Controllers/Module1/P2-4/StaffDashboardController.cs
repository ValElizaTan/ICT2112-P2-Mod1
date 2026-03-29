using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ProRental.Domain.Enums;
using ProRental.Domain.Entities;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24;

public class StaffDashboardController : Controller
{
    private readonly StaffDashboardControl _staffDashboardControl;
    private readonly ShipmentControl _shipmentControl;

    public StaffDashboardController(StaffDashboardControl staffDashboardControl, IOrderTrackingService orderTrackingService, ShipmentControl shipmentControl)
    {
        _staffDashboardControl = staffDashboardControl;
        _shipmentControl = shipmentControl;
    }

    private bool IsStaff()
    {
        var role = HttpContext.Session.GetString("UserRole");
        return !string.IsNullOrEmpty(role) &&
               (role.Equals("STAFF", StringComparison.OrdinalIgnoreCase) ||
                role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Get orders, inventory items, and staff info for dashboard display
        var orders = _staffDashboardControl.GetAllOrders() ?? new List<Order>();
        var readyItems = _staffDashboardControl.GetInventoryItemsByStatus(InventoryStatus.AVAILABLE);

        ViewBag.Orders = orders;
        ViewBag.InventoryItems = readyItems;
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        // ── Staff info ──────────────────────────────────────────────
        ViewBag.StaffName = HttpContext.Session.GetString("UserName") ?? "Staff";
        ViewBag.StaffRole = HttpContext.Session.GetString("UserRole") ?? "Staff";
        ViewBag.UnreadNotifications = 0;
        ViewBag.Notifications = Enumerable.Empty<dynamic>();

        // ── Orders ──────────────────────────────────────────────────
        var allOrders = _staffDashboardControl.GetAllOrders();

        var orderViewModels = allOrders.Select(o =>
        {
            var customerName = _staffDashboardControl.GetCustomerName(o.CustomerId);
            var status = MapStatus(o.CurrentStatus);
            var itemCount = o.Orderitems?.Count ?? 0;

            return new
            {
                OrderId = o.OrderId,
                CustomerName = customerName,
                ItemCount = itemCount,
                StartDate = o.OrderDate,
                EndDate = o.OrderDate.AddDays(GetRentalDays(o.DeliveryDurationType)),
                Total = o.TotalAmount,
                Status = status,
                DispatchedOn = o.CurrentStatus == OrderStatus.DISPATCHED ? o.OrderDate : o.OrderDate,
                DaysOverdue = CalculateDaysOverdue(o.OrderDate, o.DeliveryDurationType),
                DeliveryAddress = "N/A"
            };
        }).ToList();

        // ViewBag.Orders = orderViewModels;

        ViewBag.PendingOrders = orderViewModels.Where(o => o.Status == "Pending").ToList();
        ViewBag.DispatchedOrders = orderViewModels.Where(o => o.Status == "Dispatched").ToList();
        ViewBag.OverdueOrders = orderViewModels.Where(o => o.DaysOverdue > 0 && o.Status != "Cancelled").ToList();

        ViewBag.PendingCount = orderViewModels.Count(o => o.Status == "Pending");
        ViewBag.DispatchedCount = orderViewModels.Count(o => o.Status == "Dispatched");
        ViewBag.ReadyCount = orderViewModels.Count(o => o.Status == "Ready for Dispatch");
        ViewBag.OverdueCount = ((IEnumerable<dynamic>)ViewBag.OverdueOrders).Count();

        // ── Inventory ───────────────────────────────────────────────
        var allProducts = _staffDashboardControl.GetAllProducts();
        var inventoryItems = allProducts.Select(p => new
        {
            ItemId = p.GetProductId(),
            ProductName = p.GetProductName(),
            Sku = p.GetSku(),
            Category = p.GetCategoryName(),
            Status = MapProductStatus(p.GetProductStatus()),
            WarehouseLocation = "Main Warehouse",
            UpdatedAt = DateTime.UtcNow,
            IsClearanceCandidate = false
        }).ToList();

        ViewBag.InventoryItems = inventoryItems;
        ViewBag.InventoryItemCount = inventoryItems.Count;
        ViewBag.InventoryFilter = "";

        // ── Dispatch queue (orders ready for dispatch) ──────────────
        ViewBag.DispatchQueue = orderViewModels
            .Where(o => o.Status == "Ready for Dispatch" || o.Status == "Confirmed")
            .Select(o => new
            {
                o.OrderId,
                o.CustomerName,
                DeliveryAddress = o.DeliveryAddress,
                TargetDate = o.EndDate
            }).ToList();

        // ── Returns pipeline ────────────────────────────────────────
        ViewBag.StageCounts = new Dictionary<string, int>
        {
            { "Inspection", 0 },
            { "Repair", 0 },
            { "Servicing", 0 },
            { "Cleaning", 0 },
            { "Restocked", 0 }
        };

        var shipments = _shipmentControl.GetAllShipments();
        ViewBag.ShippingAgents = Enumerable.Empty<dynamic>();
        ViewBag.ActiveShipments = shipments.Select(s =>
{
    var info = s.GetShipmentInfo();
    return new
    {
        OrderId = info.GetTrackingId(),
        AgentName = "—",
        Route = string.IsNullOrEmpty(info.GetDestinationAddress()) ? "—" : info.GetDestinationAddress(),
        Status = info.GetDispatchStatus() ? "Dispatched" : "Pending",
        Priority = "Medium",
        AgentId = 0
    };
}).Cast<object>().ToList();
        ViewBag.ShipDateFrom = "";
        ViewBag.ShipDateTo = "";
        ViewBag.ShipRoute = "";

        // ── Loans & Clearance ───────────────────────────────────────
        ViewBag.LoanBatches = Enumerable.Empty<dynamic>();
        ViewBag.ActiveClearanceBatchCount = 0;
        ViewBag.ClearanceCandidateCount = 0;
        ViewBag.ClearanceOverrideCount = 0;
        ViewBag.ClearanceUpsellCount = 0;
        ViewBag.RecentClearanceBatches = Enumerable.Empty<dynamic>();

        // ── Carbon ──────────────────────────────────────────────────
        ViewBag.CarbonKgCo2 = "-";
        ViewBag.CarbonTrend = "No data yet";

        // ── Analytics ───────────────────────────────────────────────
        ViewBag.AnalyticsWidgets = Enumerable.Empty<dynamic>();

        return View();
    }


    [HttpGet]
    public IActionResult FilterShipments(string? dateFrom, string? dateTo, string? route)
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        // ── Staff info ──────────────────────────────────────────────
        ViewBag.StaffName = HttpContext.Session.GetString("UserName") ?? "Staff";
        ViewBag.StaffRole = HttpContext.Session.GetString("UserRole") ?? "Staff";
        ViewBag.UnreadNotifications = 0;
        ViewBag.Notifications = Enumerable.Empty<dynamic>();

        // ── Orders (minimal - keep dashboard functional) ────────────
        var allOrders = _staffDashboardControl.GetAllOrders();
        var orderViewModels = allOrders.Select(o =>
        {
            var status = MapStatus(o.CurrentStatus);
            return new
            {
                OrderId = o.OrderId,
                CustomerName = _staffDashboardControl.GetCustomerName(o.CustomerId),
                ItemCount = o.Orderitems?.Count ?? 0,
                StartDate = o.OrderDate,
                EndDate = o.OrderDate.AddDays(GetRentalDays(o.DeliveryDurationType)),
                Total = o.TotalAmount,
                Status = status,
                DispatchedOn = o.OrderDate,
                DaysOverdue = CalculateDaysOverdue(o.OrderDate, o.DeliveryDurationType),
                DeliveryAddress = "N/A"
            };
        }).ToList();

        ViewBag.Orders = allOrders;
        ViewBag.PendingOrders = orderViewModels.Where(o => o.Status == "Pending").ToList();
        ViewBag.DispatchedOrders = orderViewModels.Where(o => o.Status == "Dispatched").ToList();
        ViewBag.OverdueOrders = orderViewModels.Where(o => o.DaysOverdue > 0 && o.Status != "Cancelled").ToList();
        ViewBag.PendingCount = orderViewModels.Count(o => o.Status == "Pending");
        ViewBag.DispatchedCount = orderViewModels.Count(o => o.Status == "Dispatched");
        ViewBag.ReadyCount = orderViewModels.Count(o => o.Status == "Ready for Dispatch");
        ViewBag.OverdueCount = ((IEnumerable<dynamic>)ViewBag.OverdueOrders).Count();

        // ── Inventory ───────────────────────────────────────────────
        var allProducts = _staffDashboardControl.GetAllProducts();
        var inventoryItems = allProducts.Select(p => new
        {
            ItemId = p.GetProductId(),
            ProductName = p.GetProductName(),
            Sku = p.GetSku(),
            Category = p.GetCategoryName(),
            Status = MapProductStatus(p.GetProductStatus()),
            WarehouseLocation = "Main Warehouse",
            UpdatedAt = DateTime.UtcNow,
            IsClearanceCandidate = false
        }).ToList();

        ViewBag.InventoryItems = inventoryItems;
        ViewBag.InventoryItemCount = inventoryItems.Count;
        ViewBag.InventoryFilter = "";
        ViewBag.DispatchQueue = orderViewModels
            .Where(o => o.Status == "Ready for Dispatch" || o.Status == "Confirmed")
            .Select(o => new { o.OrderId, o.CustomerName, DeliveryAddress = o.DeliveryAddress, TargetDate = o.EndDate })
            .ToList();

        // ── Shipping filter ─────────────────────────────────────────
        var allShipments = _shipmentControl.GetAllShipments();

        var fromDate = ParseFlexibleDate(dateFrom);
        var toDate = ParseFlexibleDate(dateTo);
        var routeFilter = route?.Trim();

        // Filter by route/destination and order date range.
        // Shipments are linked to orders by TrackingId -> OrderId.
        var filtered = allShipments
            .Where(s =>
            {
                var info = s.GetShipmentInfo();
                if (!string.IsNullOrWhiteSpace(routeFilter) &&
                    !info.GetDestinationAddress().Contains(routeFilter, StringComparison.OrdinalIgnoreCase))
                    return false;

                if (fromDate.HasValue || toDate.HasValue)
                {
                    var relatedOrder = allOrders.FirstOrDefault(o => o.OrderId == info.GetTrackingId());
                    if (relatedOrder == null)
                        return false;

                    var shipDate = relatedOrder.OrderDate.Date;

                    if (fromDate.HasValue && shipDate < fromDate.Value.Date)
                        return false;

                    if (toDate.HasValue && shipDate > toDate.Value.Date)
                        return false;
                }

                return true;
            })
            .Select(s =>
            {
                var info = s.GetShipmentInfo();
                return new
                {
                    OrderId   = info.GetTrackingId(),
                    AgentName = "—",
                    Route     = string.IsNullOrEmpty(info.GetDestinationAddress()) ? "—" : info.GetDestinationAddress(),
                    Status    = info.GetDispatchStatus() ? "Dispatched" : "Pending",
                    Priority  = "Medium",
                    AgentId   = 0
                };
            })
            .Cast<object>()
            .ToList();

        ViewBag.ShippingAgents  = Enumerable.Empty<dynamic>();
        ViewBag.ActiveShipments = filtered;
        ViewBag.ShipDateFrom    = fromDate?.ToString("yyyy-MM-dd") ?? "";
        ViewBag.ShipDateTo      = toDate?.ToString("yyyy-MM-dd") ?? "";
        ViewBag.ShipRoute       = route    ?? "";

        // ── Remaining stubs ─────────────────────────────────────────
        ViewBag.StageCounts = new Dictionary<string, int>
        {
            { "Inspection", 0 }, { "Repair", 0 }, { "Servicing", 0 },
            { "Cleaning", 0 }, { "Restocked", 0 }
        };
        ViewBag.LoanBatches = Enumerable.Empty<dynamic>();
        ViewBag.ActiveClearanceBatchCount = 0;
        ViewBag.ClearanceCandidateCount   = 0;
        ViewBag.ClearanceOverrideCount    = 0;
        ViewBag.ClearanceUpsellCount      = 0;
        ViewBag.RecentClearanceBatches    = Enumerable.Empty<dynamic>();
        ViewBag.CarbonKgCo2    = "-";
        ViewBag.CarbonTrend    = "No data yet";
        ViewBag.AnalyticsWidgets = Enumerable.Empty<dynamic>();

        return View("Index");
    }

    public IActionResult OnNavigateToWalkIn(string type = "new")
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        if (type == "existing")
            return RedirectToAction("SelectExistingCustomer", "WalkInOrder");

        return RedirectToAction("EnterCustomerDetails", "WalkInOrder");
    }

    public IActionResult OnNavigateToShipping()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        return RedirectToAction("DisplayShipmentList", "Shipping");
    }

    public IActionResult OnNavigateToStaffProfile()
    {
        if (!IsStaff()) return RedirectToAction("StaffLogin", "Module1");

        return RedirectToAction("Index", "StaffProfile");
    }

    public IActionResult OnLogout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private static string MapStatus(OrderStatus? status) => status switch
    {
        OrderStatus.PENDING => "Pending",
        OrderStatus.CONFIRMED => "Confirmed",
        OrderStatus.PROCESSING => "Processing",
        OrderStatus.READY_FOR_DISPATCH => "Ready for Dispatch",
        OrderStatus.DISPATCHED => "Dispatched",
        OrderStatus.DELIVERED => "Returned",
        OrderStatus.CANCELLED => "Cancelled",
        _ => "Unknown"
    };

    private static string MapProductStatus(ProductStatus status) => status switch
    {
        ProductStatus.AVAILABLE => "Ready",
        ProductStatus.UNAVAILABLE => "Rented",
        ProductStatus.RETIRED => "Damaged",
        _ => "Unknown"
    };

    private static int GetRentalDays(DeliveryDuration? duration) => duration switch
    {
        DeliveryDuration.NextDay => 1,
        DeliveryDuration.ThreeDays => 3,
        DeliveryDuration.OneWeek => 7,
        _ => 7
    };

    private static int CalculateDaysOverdue(DateTime orderDate, DeliveryDuration? duration)
    {
        var endDate = orderDate.AddDays(GetRentalDays(duration));
        var overdue = (DateTime.UtcNow - endDate).Days;
        return overdue > 0 ? overdue : 0;
    }

    private static DateTime? ParseFlexibleDate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" };
        if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            return parsed;

        return DateTime.TryParse(input, out parsed) ? parsed : (DateTime?)null;
    }
}
