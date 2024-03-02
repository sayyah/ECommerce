using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogAuthorRepository : IRepositoryBase<BlogAuthor>
{
    PagedList<BlogAuthor> Search(PaginationParameters paginationParameters);
    Task<BlogAuthor?> GetByName(string name, CancellationToken cancellationToken);
}
