using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

public partial class Product
{
    public int      GetProductId()    => _productid;
    public int      GetCategoryId()   => _categoryid;
    public string   GetSku()          => _sku;

    // ✅ FIX: use Productdetail METHODS (not properties)
    // public string   GetProductName()  => Productdetail?.GetName() ?? "(unnamed)";
    // public string?  GetDescription()  => Productdetail?.GetDescription();
    public decimal? GetDepositRate()  => null; // optional (no getter yet)
    public decimal? GetWeight()       => null; // optional (no getter yet)
    // public string?  GetImageUrl()     => Productdetail?.GetImage();

    public string   GetCategoryName() => Category?.CategoryName ?? string.Empty;

    public ProductStatus GetProductStatus() => Status;
}