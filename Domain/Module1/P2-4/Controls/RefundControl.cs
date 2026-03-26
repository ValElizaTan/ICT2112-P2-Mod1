using System.Reflection;
using ProRental.Data.Module1.Interfaces;
using ProRental.Domain.Entities;
using ProRental.Domain.Enums;
using ProRental.Domain.Module1.P24.Interfaces;
using Team6 = ProRental.Interfaces.Domain;

namespace ProRental.Domain.Module1.P24.Controls;

public class RefundControl : IRefundService
{
    private readonly IRefundGateway _refundGateway;
    private readonly Team6.IOrderService _orderService;
    private readonly Team6.IInventoryService _inventoryService;

    public RefundControl(
        IRefundGateway refundGateway,
        Team6.IOrderService orderService,
        Team6.IInventoryService inventoryService)
    {
        _refundGateway = refundGateway;
        _orderService = orderService;
        _inventoryService = inventoryService;
    }

    public List<Refund> GetCustomerRefunds(int customerId)
    {
        try
        {
            return _refundGateway.GetRefundsByCustomerId(customerId);
        }
        catch
        {
            return new List<Refund>();
        }
    }

    public List<Returnrequest> GetCustomerReturns(int customerId)
    {
        try
        {
            return _refundGateway.GetReturnRequestsByCustomerId(customerId);
        }
        catch
        {
            return new List<Returnrequest>();
        }
    }

    public Refund? GetRefundByOrderId(int orderId)
    {
        return _refundGateway.GetRefundByOrderId(orderId);
    }

    public List<Order> GetAllOrders()
    {
        return _orderService.GetOrders() ?? new List<Order>();
    }

    public Order? GetOrder(int orderId)
    {
        return _orderService.GetOrder(orderId);
    }

    public bool InitiateReturn(int orderId, int customerId, string returnMethod)
    {
        // 1. Create a ReturnRequest record
        var returnRequest = CreateReturnRequest(orderId, customerId);
        _refundGateway.InsertReturnRequest(returnRequest);

        // 2. Update order status to PROCESSING (awaiting inspection)
        _orderService.UpdateOrderStatus(orderId, OrderStatus.PROCESSING);

        // 3. Trigger Module 2's return process (inspection, cleaning, restocking)
        var inspectionPassed = _inventoryService.TriggerReturnProcess(orderId);

        // 4. If inspection passed (faked as immediate), process the refund
        if (inspectionPassed)
        {
            var returnRequestId = GetPrivateField<int>(returnRequest, "_returnrequestid");
            var depositAmount = GetDepositAmount(orderId);
            var refund = CreateRefund(orderId, customerId, returnRequestId, depositAmount, returnMethod);
            _refundGateway.InsertRefund(refund);

            _orderService.UpdateOrderStatus(orderId, OrderStatus.DELIVERED);
        }

        return true;
    }

    private decimal GetDepositAmount(int orderId)
    {
        var order = _orderService.GetOrder(orderId);
        if (order?.Deposits != null && order.Deposits.Any())
        {
            return order.Deposits.Sum(d => GetPrivateField<decimal>(d, "_heldamount"));
        }
        // Fallback: no deposit found
        return 0m;
    }

    private static Returnrequest CreateReturnRequest(int orderId, int customerId)
    {
        var rr = new Returnrequest();
        SetPrivateField(rr, "_orderid", orderId);
        SetPrivateField(rr, "_customerid", customerId);
        SetPrivateField(rr, "_requestdate", DateTime.UtcNow);
        SetPrivateField(rr, "_status", ReturnRequestStatus.PROCESSING);
        return rr;
    }

    private static Refund CreateRefund(int orderId, int customerId, int returnRequestId, decimal amount, string method)
    {
        var refund = new Refund();
        SetPrivateField(refund, "_orderid", orderId);
        SetPrivateField(refund, "_customerid", customerId);
        SetPrivateField(refund, "_returnrequestid", returnRequestId);
        SetPrivateField(refund, "_depositrefundamount", amount);
        SetPrivateField(refund, "_returndate", DateTime.UtcNow);
        SetPrivateField(refund, "_returnmethod", method);
        return refund;
    }

    private static void SetPrivateField<T>(object obj, string fieldName, T value)
    {
        var field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    private static T? GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance);
        return field != null ? (T?)field.GetValue(obj) : default;
    }
}
