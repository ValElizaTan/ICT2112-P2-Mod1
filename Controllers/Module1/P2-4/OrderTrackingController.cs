using Microsoft.AspNetCore.Mvc;
using ProRental.Domain.Module1.P24.Controls;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;

namespace ProRental.Controllers
{
    public class OrderTrackingController : Controller
    {
        private readonly IOrderTrackingService _control;

        // Displayed fields from class diagram
        private int _displayedOrderId;
        private int _displayedOrderItemId;
        private OrderStatus _displayedStatus;
        private List<OrderStatusHistory> _displayedTimeline = new();
        private List<Orderitem> _filteredItems = new();
        private string _message = string.Empty;

        public OrderTrackingController(IOrderTrackingService control)
        {
            _control = control;
        }

        public bool ValidateOrderItemId(int orderItemId)
        {
            return _control.GetOrderItemById(orderItemId) != null;
        }

        public OrderStatus ShowCurrentOrderStatus(int orderId)
        {
            return _control.GetCurrentOrderStatus(orderId);
        }

        public OrderStatus ShowCurrentItemStatus(int orderItemId)
        {
            return _control.GetCurrentItemStatus(orderItemId);
        }

        public List<OrderStatusHistory> ShowTimeline(int orderItemId)
        {
            return _control.GetTimeline(orderItemId);
        }

        public void ShowMessage(string message)
        {
            _message = message;
            TempData["Message"] = message;
        }

        [HttpGet]
        public IActionResult CustomerOrderTracking(int? orderId, int? selectedItemId)
        {
            // HARD-CODED TEMP: simulate logged-in customer
            int currentCustomerId = 1;

            var customer = _control.GetCustomerById(currentCustomerId);
            var customerOrders = _control.GetOrdersByCustomer(currentCustomerId);

            if (!customerOrders.Any())
            {
                ShowMessage("No orders found for current customer.");
                ViewBag.Customer = customer;
                ViewBag.CustomerOrders = customerOrders;
                ViewBag.OrderItems = new List<Orderitem>();
                ViewBag.Timeline = new List<OrderStatusHistory>();
                return View();
            }

            _displayedOrderId = orderId ?? customerOrders.First().OrderId;

            var order = _control.GetOrderById(_displayedOrderId);
            if (order == null || order.CustomerId != currentCustomerId)
            {
                ShowMessage("Invalid order ID or access denied.");
                return RedirectToAction(nameof(CustomerOrderTracking), new { orderId = customerOrders.First().OrderId });
            }

            var items = _control.GetItemsByOrder(_displayedOrderId);
            var currentOrderStatus = ShowCurrentOrderStatus(_displayedOrderId);

            _displayedOrderItemId = selectedItemId ?? items.FirstOrDefault()?.OrderItemId ?? 0;

            if (_displayedOrderItemId != 0 && ValidateOrderItemId(_displayedOrderItemId))
            {
                _displayedTimeline = ShowTimeline(_displayedOrderItemId);
            }

            ViewBag.Customer = customer;
            ViewBag.CustomerOrders = customerOrders;
            ViewBag.SelectedOrder = order;
            ViewBag.OrderItems = items;
            ViewBag.CurrentOrderStatus = currentOrderStatus;
            ViewBag.SelectedItemId = _displayedOrderItemId;
            ViewBag.Timeline = _displayedTimeline;
            ViewBag.StatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();

            return View();
        }

        [HttpGet]
        public IActionResult StaffOrderTracking(int? orderId, string? filterStatus, int? selectedItemId)
        {
            // HARD-CODED TEMP: simulate logged-in staff
            int currentStaffId = 101;

            var staff = _control.GetStaffById(currentStaffId);
            if (staff == null)
            {
                ShowMessage("Invalid staff account.");
                return View();
            }

            // Hardcoded default order for demo
            _displayedOrderId = orderId ?? 1001;

            var order = _control.GetOrderById(_displayedOrderId);
            var items = _control.GetItemsByOrder(_displayedOrderId);

            if (order == null)
            {
                ShowMessage("Invalid order ID.");
                ViewBag.OrderItems = new List<Orderitem>();
                ViewBag.Timeline = new List<OrderStatusHistory>();
                return View();
            }

            if (!string.IsNullOrWhiteSpace(filterStatus) &&
                Enum.TryParse<OrderStatus>(filterStatus, out var parsedStatus))
            {
                _filteredItems = _control.FilterItemsByStatus(parsedStatus)
                    .Where(x => x.OrderId == _displayedOrderId)
                    .ToList();

                if (!_filteredItems.Any())
                {
                    ShowMessage($"No order items found with status {parsedStatus} for this order.");
                }
            }
            else
            {
                _filteredItems = items;
            }

            _displayedOrderItemId = selectedItemId ?? _filteredItems.FirstOrDefault()?.OrderItemId ?? 0;

            if (_displayedOrderItemId != 0 && ValidateOrderItemId(_displayedOrderItemId))
            {
                _displayedTimeline = ShowTimeline(_displayedOrderItemId);
                _displayedStatus = ShowCurrentItemStatus(_displayedOrderItemId);
            }

            ViewBag.Staff = staff;
            ViewBag.SelectedOrder = order;
            ViewBag.OrderItems = _filteredItems;
            ViewBag.CurrentOrderStatus = ShowCurrentOrderStatus(_displayedOrderId);
            ViewBag.SelectedItemId = _displayedOrderItemId;
            ViewBag.CurrentItemStatus = _displayedStatus;
            ViewBag.Timeline = _displayedTimeline;
            ViewBag.StatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
            ViewBag.SelectedFilterStatus = filterStatus ?? string.Empty;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int orderId, int orderItemId, OrderStatus newStatus, string? remark)
        {
            // HARD-CODED TEMP: simulate logged-in staff
            int currentStaffId = 101;

            try
            {
                OnClickUpdateStatus(orderItemId, newStatus, remark ?? string.Empty, currentStaffId);
                ShowMessage($"Order item {orderItemId} updated to {newStatus} successfully.");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

            return RedirectToAction(nameof(StaffOrderTracking), new { orderId, selectedItemId = orderItemId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkUpdate(int orderId, string selectedOrderItemIds, OrderStatus newStatus, string? remark)
        {
            // HARD-CODED TEMP: simulate logged-in staff
            int currentStaffId = 101;

            var orderItemIds = new List<int>();

            if (!string.IsNullOrWhiteSpace(selectedOrderItemIds))
            {
                orderItemIds = selectedOrderItemIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();
            }

            if (!orderItemIds.Any())
            {
                ShowMessage("Please select at least one order item for bulk update.");
                return RedirectToAction(nameof(StaffOrderTracking), new { orderId });
            }

            var results = OnClickBulkUpdate(orderItemIds, newStatus, remark ?? string.Empty, currentStaffId);
            TempData["BulkResults"] = string.Join("||", results);

            return RedirectToAction(nameof(StaffOrderTracking), new { orderId });
        }

        public void OnClickUpdateStatus(int orderItemId, OrderStatus newStatus, string remark, int staffId)
        {
            _control.UpdateStatus(orderItemId, newStatus, remark, staffId);
        }

        public List<string> OnClickBulkUpdate(List<int> orderItemIds, OrderStatus newStatus, string remark, int staffId)
        {
            return _control.BulkUpdateStatus(orderItemIds, newStatus, remark, staffId);
        }
    }
}