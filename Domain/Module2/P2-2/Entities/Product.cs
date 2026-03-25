using ProRental.Domain.Enums;

namespace ProRental.Domain.Entities;

/// <summary>
/// Partial extension for the auto-generated Product entity.
///
/// Calls public properties from:
///   - Productdetail.partial.cs  (ProductName, ProductPrice, etc.)
///   - Category.partial.cs       (CategoryName)
///
/// NOTE: GetPrice() is defined ONLY here (in the Domain\Module6 partial).
///       Any other partial that defined GetPrice() must remove it.
/// </summary>
public partial class Product
{
    public int      GetProductId()    => _productid;
    public int      GetCategoryId()   => _categoryid;
    public string   GetSku()          => _sku;

    // Reads from Productdetail partial public properties
    public string   GetProductName()  => Productdetail?.ProductName        ?? "(unnamed)";
    public string?  GetDescription()  => Productdetail?.ProductDesc;
    public decimal? GetDepositRate()  => Productdetail?.ProductDepositRate;
    public decimal? GetWeight()       => Productdetail?.ProductWeight;
    public string?  GetImageUrl()     => Productdetail?.ProductImage;

    // Reads from Category partial — uses the CategoryName convenience property
    public string   GetCategoryName() => Category?.CategoryName            ?? string.Empty;

    // ── Status ────────────────────────────────────────────────────────────
    // The auto-generated entity exposes Status as a public property (ProductStatus enum).
    // We surface it through the named accessor expected by the rest of the codebase.
    public ProductStatus GetProductStatus() => Status;
}