namespace ProRental.Domain.Entities;

/// <summary>Generic paged result wrapper for catalogue listings.</summary>
public class PagedResult<T>
{
    public List<T> Items       { get; set; } = new();
    public int     TotalCount  { get; set; }
    public int     CurrentPage { get; set; }
    public int     PageSize    { get; set; }
    public int     TotalPages  => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool    HasPrev     => CurrentPage > 1;
    public bool    HasNext     => CurrentPage < TotalPages;
}