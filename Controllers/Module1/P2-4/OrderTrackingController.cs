using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24
{
    public class OrderTrackingController : Controller
    {
        private readonly IOrderTrackingService _orderTrackingService;

        // DEMO MODE ONLY
        private const int DemoCustomerId = 1;
        private const int DemoStaffId = 1;

        public OrderTrackingController(IOrderTrackingService orderTrackingService)
        {
            _orderTrackingService = orderTrackingService;
        }

        // =========================================================
        // CUSTOMER VIEW
        // =========================================================
        [HttpGet]
        public IActionResult CustomerOrderTracking(int? timelineOrderId = null)
        {
            int customerId = ResolveCurrentCustomerId();

            var orders = _orderTrackingService.GetOrdersByCustomerId(customerId) ?? new List<Order>();

            ViewBag.CustomerId = customerId;
            ViewBag.Orders = orders;
            ViewBag.Message = TempData["Message"]?.ToString();
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.SelectedTimelineOrderId = timelineOrderId;

            if (timelineOrderId.HasValue)
            {
                var selectedOrder = _orderTrackingService.SearchOrderById(timelineOrderId.Value);

                if (selectedOrder == null || selectedOrder.GetCustomerId() != customerId)
                {
                    TempData["ErrorMessage"] = "You are not allowed to view this timeline.";
                    return RedirectToAction(nameof(CustomerOrderTracking));
                }

                ViewBag.SelectedTimeline = _orderTrackingService.GetOrderTimeline(timelineOrderId.Value)
                                        ?? new List<Orderstatushistory>();

                return View("~/Views/Module1/P2-4/OrderTracking/CustomerOrderTimeline.cshtml");
            }

            ViewBag.SelectedTimeline = new List<Orderstatushistory>();
            return View("~/Views/Module1/P2-4/OrderTracking/CustomerOrderTracking.cshtml");
        }

        // =========================================================
        // STAFF VIEW
        // =========================================================
        [HttpGet]
        public IActionResult StaffOrderTracking(int? orderId = null, OrderStatus? status = null, int? timelineOrderId = null)
        {
            int staffId = ResolveCurrentStaffId();

            if (!_orderTrackingService.HasUpdatePermission(staffId))
            {
                TempData["ErrorMessage"] = "Staff does not have permission.";
                return View("~/Views/Module1/P2-4/OrderTracking/StaffOrderTracking.cshtml");
            }

            List<Order> orders;

            if (orderId.HasValue)
            {
                var foundOrder = _orderTrackingService.SearchOrderById(orderId.Value);
                orders = foundOrder != null ? new List<Order> { foundOrder } : new List<Order>();
            }
            else if (status.HasValue)
            {
                orders = _orderTrackingService.FilterOrdersByStatus(status.Value) ?? new List<Order>();
            }
            else
            {
                orders = _orderTrackingService.GetAllOrders() ?? new List<Order>();
            }

            ViewBag.StaffId = staffId;
            ViewBag.Orders = orders;
            ViewBag.SelectedOrderId = orderId;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedTimelineOrderId = timelineOrderId;
            ViewBag.Message = TempData["Message"]?.ToString();
            ViewBag.ErrorMessage = TempData["ErrorMessage"]?.ToString();
            ViewBag.BulkResults = ParseBulkResults(TempData["BulkResults"]?.ToString());

            if (timelineOrderId.HasValue)
            {
                ViewBag.SelectedTimeline = _orderTrackingService.GetOrderTimeline(timelineOrderId.Value)
                                        ?? new List<Orderstatushistory>();

                return View("~/Views/Module1/P2-4/OrderTracking/StaffUpdateOrderStatus.cshtml");
            }

            ViewBag.SelectedTimeline = new List<Orderstatushistory>();
            return View("~/Views/Module1/P2-4/OrderTracking/StaffOrderTracking.cshtml");
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, OrderStatus newStatus, string? remark)
        {
            int staffId = ResolveCurrentStaffId();

            try
            {
                _orderTrackingService.UpdateStatus(orderId, newStatus, remark, staffId);
                TempData["Message"] = $"Order {orderId} updated to {newStatus}.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(StaffOrderTracking), new
            {
                orderId = orderId,
                timelineOrderId = orderId
            });
        }

        [HttpPost]
        public IActionResult BulkUpdateOrderStatus(List<int> orderIds, OrderStatus newStatus, string? remark)
        {
            int staffId = ResolveCurrentStaffId();

            try
            {
                var safeOrderIds = orderIds ?? new List<int>();
                var results = _orderTrackingService.BulkUpdateStatus(safeOrderIds, newStatus, remark, staffId);
                TempData["BulkResults"] = string.Join("||", results ?? new List<string>());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(StaffOrderTracking));
        }

        [HttpGet]
        public IActionResult FilterOrdersByStatus(OrderStatus status)
        {
            return RedirectToAction(nameof(StaffOrderTracking), new { status });
        }

        [HttpGet]
        public IActionResult SearchOrderById(int orderId)
        {
            return RedirectToAction(nameof(StaffOrderTracking), new { orderId });
        }

        // =========================================================
        // HELPERS
        // =========================================================
        [NonAction]
        private int ResolveCurrentCustomerId()
        {
            // DEMO MODE ONLY
            return DemoCustomerId;
        }

        [NonAction]
        private int ResolveCurrentStaffId()
        {
            // DEMO MODE ONLY
            return DemoStaffId;
        }

        [NonAction]
        private List<string> ParseBulkResults(string? bulkRaw)
        {
            if (string.IsNullOrWhiteSpace(bulkRaw))
                return new List<string>();

            return bulkRaw
                .Split("||", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}