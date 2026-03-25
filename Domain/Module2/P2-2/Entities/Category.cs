namespace ProRental.Domain.Entities;

/// <summary>
/// Partial extension for the auto-generated Category entity.
///
/// The auto-generated file uses PRIVATE backing fields (_categoryid, _name, _description).
/// Accessing them is legal here because this is the same partial class.
/// Note: the auto-generated Category has no IsActive column — omitted.
/// </summary>
public partial class Category
{
    // Used by Product.partial.cs: Category?.CategoryName
    public string CategoryName => _name;

    // Named accessor methods matching the class diagram
    public int     GetCategoryId()  => _categoryid;
    public string  GetName()        => _name;
    public string? GetDescription() => _description;
}