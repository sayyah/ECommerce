using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCategoryRepository : IRepositoryBase<BlogCategory>
{
    Task<PagedList<BlogCategory>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken);

    Task<List<BlogCategory>?> Parents(int blogId, CancellationToken cancellationToken);
}
