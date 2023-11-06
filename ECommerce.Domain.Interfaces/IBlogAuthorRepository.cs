using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogAuthorRepository : IAsyncRepository<BlogAuthor>
{
    Task<PagedList<BlogAuthor>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<BlogAuthor> GetByName(string name, CancellationToken cancellationToken);
}
