namespace ProRental.Domain.Entities;

/// <summary>
/// Holds all user-supplied filter/search/sort options for the catalogue page.
/// </summary>
public class SearchFilter
{
    public string? Keywords    { get; set; }
    public int?    CategoryId  { get; set; }
    public decimal? MinPrice   { get; set; }
    public decimal? MaxPrice   { get; set; }
    public string  SortBy      { get; set; } = "name_asc";
    public int     CurrentPage { get; set; } = 1;
    public int     PageSize    { get; set; } = 9;
}