using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;


public partial class Product
{
    private string _name = "";

    public string GetName() => _name;

    public void SetName(string name)
    {
        _name = name;
    }
    private ProductStatus _status;
    private ProductStatus Status { get => _status; set => _status = value; }
    private decimal _price;

    public decimal GetPrice() => _price;

    public void SetPrice(decimal price)
    {
        _price = price;
    }
    public void UpdateStatus(ProductStatus status) => _status = status;

    // Add these:
    public Productdetail? ProductDetail { get; set; }
    public Product GetProduct() => this;

}