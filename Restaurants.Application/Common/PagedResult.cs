namespace Restaurants.Application.Common;
public sealed class PagedResult<T>(IReadOnlyList<T> items, int totalItems, int pageNumber, int pageSize)
{
    public IReadOnlyList<T> Items { get; } = items;
    public int TotalItems { get; } = totalItems;
    public int TotalPages { get; } = (int)Math.Ceiling(totalItems / (double)pageSize);
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int ItemsFrom { get; } = totalItems == 0 ? 0 : (pageNumber - 1) * pageSize + 1;
    public int ItemsTo { get; } = totalItems == 0 ? 0 : Math.Min(pageNumber * pageSize, totalItems);
}
