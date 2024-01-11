using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Objects;

public static class PagingExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable,
        PageViewModel pageViewModel) where T : BaseEntity
    {
        return queryable.OrderByDescending(x => x.Id)
            .Skip((pageViewModel.Page - 1) * pageViewModel.QuantityPerPage)
            .Take(pageViewModel.QuantityPerPage);
    }
}

public class PagedList<T> : List<T>
{
    public PagedList(List<T> items, int count, int pageNumber, int pageSize, string? search = null)
    {
        Search= search;
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public string? Search { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public List<ProductIndexPageViewModel> Items { get; }
    public int Count { get; }
    public int PageNumber { get; }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, string? search = null)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize, search);
    }
}
