using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCategoryRepository : IRepositoryBase<BlogCategory>
{
    IQueryable<BlogCategory> Search(PaginationParameters paginationParameters);

    Task<BlogCategory?> GetByName(string name, int? parentId, CancellationToken cancellationToken);

    Task<List<BlogCategory>?> Parents(int blogId, CancellationToken cancellationToken);
}
