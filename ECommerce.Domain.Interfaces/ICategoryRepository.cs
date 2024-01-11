using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    PagedList<Category> Search(PaginationParameters paginationParameters);

    Task<Category?> GetByName(string name, CancellationToken cancellationToken, int? parentId = null);

    Task AddAll(IEnumerable<Category> categories, CancellationToken cancellationToken);

    Task<IEnumerable<int>> GetIdsByUrl(string categoryUrl, CancellationToken cancellationToken);

    Task<CategoryViewModel?> GetByUrl(string categoryUrl, CancellationToken cancellationToken);

    Task<List<CategoryParentViewModel>?> Parents(int productId, CancellationToken cancellationToken);

    Task<List<int>> ChildrenCategory(int categoryId, CancellationToken cancellationToken);

    Task<List<Category>?> Search(string searchKeyword, CancellationToken cancellationToken);
}
