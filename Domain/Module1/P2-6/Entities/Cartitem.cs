namespace ProRental.Domain.Entities;

public partial class Cartitem
{
    private Product? _loadedProduct;

    public int GetCartItemId() => Cartitemid;

    public int GetCartId() => Cartid;

    public int GetProductId() => Productid;

    public int GetQuantity() => Quantity;

    public bool IsSelected() => Isselected ?? false;

    public void SetQuantity(int quantity)
    {
        Quantity = Math.Max(1, quantity);
    }

    public void SetSelected(bool isSelected)
    {
        Isselected = isSelected;
    }

    public void SetCartId(int cartId)
    {
        Cartid = cartId;
    }

    public void SetProductId(int productId)
    {
        Productid = productId;
    }

    // runtime-only helper for existing cart / cost calculation flow
    public Product? GetProduct()
    {
        return _loadedProduct;
    }

    public void SetProduct(Product product)
    {
        _loadedProduct = product;
    }
    public decimal GetUnitPrice()
    {
        var product = GetProduct();

        if (product == null)
            return 0m;

        if (product.Productdetail != null)
        {
            return product.Productdetail.GetPrice();
        }

        return product.GetPrice();
    }

}