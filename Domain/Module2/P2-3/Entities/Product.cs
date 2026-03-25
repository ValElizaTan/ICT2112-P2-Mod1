using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
    private ProductStatus _status;
    private ProductStatus Status { get => _status; set => _status = value; }

    public void UpdateStatus(ProductStatus status) => _status = status;

    public Product GetProduct() => this;

    // =========================
    // ✅ CORRECT: Read from Productdetail
    // =========================

    public decimal GetPrice()
    {
        return Productdetail?.GetPrice() ?? 0m;
    }

    public string GetProductName()
    {
        return Productdetail?.GetName() ?? "(unnamed)";
    }

    public string? GetDescription()
    {
        return Productdetail?.GetDescription();
    }

    public string? GetImageUrl()
    {
        return Productdetail?.GetImage();
    }

}