namespace ProRental.Domain.Entities;

public class SelectedItem
{
    private Product _product;
    private int _quantity;

    public SelectedItem(Product product, int quantity)
    {
        _product = product;
        _quantity = quantity;
    }

    public Product GetProduct() => _product;
    public void SetProduct(Product product) => _product = product;

    public int GetQuantity() => _quantity;
    public void SetQuantity(int quantity) => _quantity = quantity;

    public void IncreaseQuantity(int amount) => _quantity += amount;
    public void DecreaseQuantity(int amount) => _quantity -= amount;
    public void ValidateQuantity(int amount) { /* validation logic */ }

    public decimal GetUnitPrice()
    {
        var product = GetProduct();

        if (product.Productdetail != null)
        {
            return product.Productdetail.GetPrice();
        }

        return product.GetPrice();
    }
}