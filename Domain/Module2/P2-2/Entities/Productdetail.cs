namespace ProRental.Domain.Entities;

/// <summary>
/// Partial extension for the auto-generated Productdetail entity.
///
/// The auto-generated file declares all fields as private
/// (e.g. "private string _name; private string Name { get => _name; ... }").
/// Accessing the backing fields directly (_name, _price, etc.) is legal
/// inside the same partial class.
/// </summary>
public partial class Productdetail
{
    // Prefixed with "Product" to avoid any name collision with the
    // private auto-generated property names (Name, Price, etc.)
    public string   ProductName        => _name;
    public string?  ProductDesc        => _description;
    public decimal  ProductPrice       => _price;
    public decimal? ProductDepositRate => _depositrate;
    public decimal? ProductWeight      => _weight;
    public string?  ProductImage       => _image;
    public int      ProductTotalQty    => _totalquantity;
}