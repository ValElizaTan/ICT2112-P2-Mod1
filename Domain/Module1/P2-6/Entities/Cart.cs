namespace ProRental.Domain.Entities;

public partial class Cart
{
    private string?        _cartId;
    private string?        _sessionId;
    private string?        _customerId;
    private DateTime?      _startDate;
    private DateTime?      _endDate;
    private List<CartItem> _items = new();

    // ── Getters ──────────────────────────────────────────────────────────────
    public string?   GetCartId()    => _cartId;
    public string?   GetSessionId() => _sessionId;
    public string?   GetCustomerId() => _customerId;
    public DateTime? GetStartDate() => _startDate;
    public DateTime? GetEndDate()   => _endDate;

    public List<CartItem> GetItems() => _items;

    public List<CartItem> GetSelectedItems() =>
        _items.Where(i => i.IsSelected()).ToList();

    // ── Item operations ───────────────────────────────────────────────────────

    /// <summary>Add by productId + qty (called from CartControl.AddToCart).</summary>
    public void AddItem(int productId, int qty)
    {
        var existing = _items.FirstOrDefault(i => i.GetProductId() == productId);
        if (existing != null)
            existing.SetQuantity(existing.GetQuantity() + qty);
        else
            _items.Add(new CartItem(productId, qty));
    }

    /// <summary>Add a fully constructed CartItem (used during session merge).</summary>
    public void AddItem(CartItem item)
    {
        var existing = _items.FirstOrDefault(i => i.GetProductId() == item.GetProductId());
        if (existing != null)
            existing.SetQuantity(existing.GetQuantity() + item.GetQuantity());
        else
            _items.Add(item);
    }

    public void UpdateQuantity(int productId, int qty)
    {
        var item = _items.FirstOrDefault(i => i.GetProductId() == productId);
        if (item != null) item.SetQuantity(Math.Max(1, qty));
    }

    public void RemoveItem(int productId) =>
        _items.RemoveAll(i => i.GetProductId() == productId);

    public void EmptyCart() => _items.Clear();

    // ── Rental period ─────────────────────────────────────────────────────────
    public void SetRentalPeriod(DateTime startDate, DateTime endDate)
    {
        _startDate = startDate;
        _endDate   = endDate;
    }

    // ── Selection ─────────────────────────────────────────────────────────────
    public void ToggleSelect(int productId, bool isSelected)
    {
        var item = _items.FirstOrDefault(i => i.GetProductId() == productId);
        if (item != null) item.SetIsSelected(isSelected);
    }

    public void SelectAll()      => _items.ForEach(i => i.SetIsSelected(true));
    public void ClearSelection() => _items.ForEach(i => i.SetIsSelected(false));
}