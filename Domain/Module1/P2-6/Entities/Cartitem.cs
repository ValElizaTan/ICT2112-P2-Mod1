using System;

namespace ProRental.Domain.Entities;

public partial class Cartitem
{
    private Product? _product;

    public void SetProduct(Product product)
    {
        _product = product;
    }

    public Product GetProduct()
    {
        return _product ?? throw new InvalidOperationException(
            $"Product not loaded for Cartitem with ProductId {GetProductId()}."
        );
    }

    public void SetCartId(int cartId)
    {
        Cartid = cartId;
    }

    public void SetProductId(int productId)
    {
        Productid = productId;
    }

    public int GetCartItemId()
    {
        return Cartitemid;
    }

    public int GetCartId()
    {
        return Cartid;
    }

    public int GetProductId()
    {
        return Productid;
    }

    public int GetQuantity()
    {
        return Quantity;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");
        }

        Quantity = quantity;
    }

    public void SetSelected(bool isSelected)
    {
        Isselected = isSelected;
    }

    public bool IsSelected()
    {
        return Isselected ?? false;
    }
}