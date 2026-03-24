namespace ProRental.Domain.Entities;

public class CartItem
{
    private int     _productId;
    private int     _quantity;
    private bool    _isSelected;
    private Product? _product;

    public CartItem(int productId, int quantity, bool isSelected = true)
    {
        _productId  = productId;
        _quantity   = quantity;
        _isSelected = isSelected;
    }

    // ── Getters / Setters ─────────────────────────────────────────────────────
    public int  GetProductId()  => _productId;
    public int  GetQuantity()   => _quantity;
    public bool IsSelected()    => _isSelected;

    /// <summary>
    /// Returns the associated Product. Product is loaded/set by the mapper
    /// (e.g. via EF navigation property or explicit assignment after retrieval).
    /// </summary>
    public Product GetProduct() => _product
        ?? throw new InvalidOperationException(
               $"Product not loaded for CartItem (productId={_productId}). " +
               "Ensure the mapper populates CartItem.Product before calling GetProduct().");

    public void SetQuantity(int quantity)      => _quantity   = Math.Max(1, quantity);
    public void SetIsSelected(bool isSelected) => _isSelected = isSelected;
    public void SetProduct(Product product)    => _product    = product;
}