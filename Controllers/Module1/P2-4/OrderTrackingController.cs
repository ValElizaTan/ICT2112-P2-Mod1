using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers.Module1.P24
{
    public class OrderTrackingController : Controller
    {
        private readonly IOrderTrackingService _control;

        private int _displayedOrderId;
        private int _displayedOrderItemId;
        private OrderStatus _displayedStatus;
        private List<Orderstatushistory> _displayedTimeline = new();
        private List<Orderitem> _filteredItems = new();
        private string _message = string.Empty;

        public OrderTrackingController(IOrderTrackingService control)
        {
            _control = control;
        }

        // =========================================================
        // CUSTOMER VIEW
        // =========================================================
        [HttpGet]
        public IActionResult CustomerOrderTracking()
        {
            int customerId = ResolveCurrentCustomerId();

            var orders = ShowOrdersByCustomer(customerId);

            ViewBag.CustomerId = customerId;
            ViewBag.Orders = orders;
            ViewBag.Message = TempData["Message"]?.ToString() ?? _message;

            return View("~/Views/Module1/OrderTracking/CustomerOrderTracking.cshtml");
        }

        // =========================================================
        // STAFF VIEW
        // =========================================================
        [HttpGet]
        public IActionResult StaffOrderTracking(int? orderId = null, OrderStatus? status = null)
        {
            int staffId = ResolveCurrentStaffId();

            List<Orderitem> items;
            if (status.HasValue)
            {
                items = FilterItemsByStatus(status.Value);
            }
            else if (orderId.HasValue)
            {
                items = ShowItemsByOrder(orderId.Value);
            }
            else
            {
                items = _control.FilterItemsByStatus(OrderStatus.PACKING)
                    .Concat(_control.FilterItemsByStatus(OrderStatus.READY_FOR_DISPATCH))
                    .Concat(_control.FilterItemsByStatus(OrderStatus.DISPATCHED))
                    .Concat(_control.FilterItemsByStatus(OrderStatus.IN_RENTAL))
                    .Concat(_control.FilterItemsByStatus(OrderStatus.RETURN_PICKUP))
                    .Concat(_control.FilterItemsByStatus(OrderStatus.RETURNED))
                    .Concat(_control.FilterItemsByStatus(OrderStatus.INSPECTION))
                    .ToList();
            }

            ViewBag.StaffId = staffId;
            ViewBag.Items = items;
            ViewBag.SelectedOrderId = orderId;
            ViewBag.SelectedStatus = status;
            ViewBag.Message = TempData["Message"]?.ToString() ?? _message;

            return View("~/Views/Module1/OrderTracking/StaffOrderTracking.cshtml");
        }

        [HttpGet]
        public IActionResult Timeline(int orderItemId)
        {
            try
            {
                var item = _control.GetOrderItemById(orderItemId);
                if (item == null)
                {
                    TempData["Message"] = $"Order item {orderItemId} not found.";
                    return RedirectToAction(nameof(StaffOrderTracking));
                }

                var timeline = _control.GetTimeline(orderItemId);

                ViewBag.OrderItem = item;
                ViewBag.Timeline = timeline;

                return View("~/Views/Module1/OrderTracking/Timeline.cshtml");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction(nameof(StaffOrderTracking));
            }
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderItemId, OrderStatus newStatus, string remark)
        {
            int staffId = ResolveCurrentStaffId();

            try
            {
                OnClickUpdateStatus(orderItemId, newStatus, remark, staffId);
                TempData["Message"] = _message;
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }

            var item = _control.GetOrderItemById(orderItemId);
            return RedirectToAction(nameof(StaffOrderTracking), new { orderId = item?.GetOrderId() });
        }

        [HttpPost]
        public IActionResult BulkUpdate(List<int> orderItemIds, OrderStatus newStatus, string remark)
        {
            int staffId = ResolveCurrentStaffId();

            var results = OnClickBulkUpdate(orderItemIds ?? new List<int>(), newStatus, remark, staffId);
            TempData["Message"] = string.Join(" | ", results);

            return RedirectToAction(nameof(StaffOrderTracking));
        }

        // =========================================================
        // METHODS FROM PDF
        // =========================================================
        [NonAction]
        public bool ValidateOrderItemId(int orderItemId)
        {
            return _control.GetOrderItemById(orderItemId) != null;
        }

        [NonAction]
        public OrderStatus ShowCurrentOrderStatus(int orderId)
        {
            _displayedOrderId = orderId;
            _displayedStatus = _control.GetCurrentOrderStatus(orderId);
            return _displayedStatus;
        }

        [NonAction]
        public OrderStatus ShowCurrentItemStatus(int orderItemId)
        {
            _displayedOrderItemId = orderItemId;
            _displayedStatus = _control.GetCurrentItemStatus(orderItemId);
            return _displayedStatus;
        }

        [NonAction]
        public List<Orderstatushistory> ShowTimeline(int orderItemId)
        {
            _displayedOrderItemId = orderItemId;
            _displayedTimeline = _control.GetTimeline(orderItemId);
            return _displayedTimeline;
        }

        [NonAction]
        public void OnClickUpdateStatus(int orderItemId, OrderStatus newStatus, string remark, int staffId)
        {
            _control.UpdateStatus(orderItemId, newStatus, remark, staffId);
            ShowMessage($"Order item {orderItemId} updated to {newStatus} successfully.");
        }

        [NonAction]
        public List<string> OnClickBulkUpdate(List<int> orderItemIds, OrderStatus newStatus, string remark, int staffId)
        {
            var result = _control.BulkUpdateStatus(orderItemIds, newStatus, remark, staffId);
            _message = "Bulk update completed.";
            return result;
        }

        [NonAction]
        public List<Orderitem> FilterItemsByStatus(OrderStatus status)
        {
            _filteredItems = _control.FilterItemsByStatus(status);
            return _filteredItems;
        }

        [NonAction]
        public void ShowMessage(string message)
        {
            _message = message;
        }

        [NonAction]
        public List<Order> ShowOrdersByCustomer(int customerId)
        {
            return _control.GetOrdersByCustomer(customerId);
        }

        [NonAction]
        public List<Orderitem> ShowItemsByOrder(int orderId)
        {
            return _control.GetItemsByOrder(orderId);
        }

        // =========================================================
        // DEMO LOGIN SIMULATION
        // =========================================================
        private int ResolveCurrentCustomerId()
        {
            // DEMO MODE: authentication is not working yet
            // Simulate logged-in customer
            return 1;

            // REPLACE THIS WHEN AUTH IS WORKING
            /*
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;
            if (string.IsNullOrWhiteSpace(customerIdClaim))
            {
                throw new UnauthorizedAccessException("Customer is not logged in.");
            }
            return int.Parse(customerIdClaim);
            */
        }

        private int ResolveCurrentStaffId()
        {
            // DEMO MODE: authentication is not working yet
            // Simulate logged-in staff
            return 1;

            // REPLACE THIS WHEN AUTH IS WORKING
            /*
            var staffIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StaffId")?.Value;
            if (string.IsNullOrWhiteSpace(staffIdClaim))
            {
                throw new UnauthorizedAccessException("Staff is not logged in.");
            }
            return int.Parse(staffIdClaim);
            */
        }
    }
}