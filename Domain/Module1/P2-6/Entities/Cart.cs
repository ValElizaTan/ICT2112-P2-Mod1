using System;
using System.Collections.Generic;
using System.Linq;
using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Cart
{
    private CartStatus _status;
    private CartStatus Status { get => _status; set => _status = value; }

    public void SetSessionId(int sessionId)
    {
        Sessionid = sessionId;
    }

    public void SetCustomerId(int? customerId)
    {
        Customerid = customerId;
    }

    public void SetStatus(CartStatus status)
    {
        Status = status;
    }

    public CartStatus GetStatus()
    {
        return Status;
    }

    public void MarkActive()
    {
        Status = CartStatus.ACTIVE;
    }

    public void MarkExpired()
    {
        Status = CartStatus.EXPIRED;
    }

    public bool IsActive()
    {
        return Status == CartStatus.ACTIVE;
    }

    public bool IsExpired()
    {
        return Status == CartStatus.EXPIRED;
    }

    public void AddItem(Cartitem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Cartitems.Add(item);
    }

    public void RemoveItem(int productId)
    {
        var item = GetItem(productId);
        if (item != null)
        {
            Cartitems.Remove(item);
        }
    }

    public void EmptyCart()
    {
        Cartitems.Clear();
    }

    public void AssignToCustomer(int customerId)
    {
        Customerid = customerId;
        Sessionid = null;
    }

    public Cartitem? GetItem(int productId)
    {
        return Cartitems.FirstOrDefault(i => i.GetProductId() == productId);
    }

    public List<Cartitem> GetItems()
    {
        return Cartitems.ToList();
    }

    public bool IsEmpty()
    {
        return !Cartitems.Any();
    }

    public int GetTotalQuantity()
    {
        return Cartitems.Sum(i => i.GetQuantity());
    }

    public int GetCartId()
    {
        return Cartid;
    }

    public int? GetCustomerId()
    {
        return Customerid;
    }

    public int? GetSessionId()
    {
        return Sessionid;
    }

    public void SetRentalPeriod(DateTime start, DateTime end)
    {
        Rentalstart = start;
        Rentalend = end;
    }

    public DateTime GetRentalStart()
    {
        return Rentalstart ?? DateTime.MinValue;
    }

    public DateTime GetRentalEnd()
    {
        return Rentalend ?? DateTime.MinValue;
    }
}