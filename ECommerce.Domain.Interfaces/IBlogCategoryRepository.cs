using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCategoryRepository : IRepositoryBase<BlogCategory>
{
    Task<PagedList<BlogCategory>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken);

    Task<List<CategoryParentViewModel>?> Parents(int blogId, CancellationToken cancellationToken);
}
