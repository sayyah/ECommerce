using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCommentRepository : IAsyncRepository<BlogComment>
{
    Task<PagedList<BlogComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PagedList<BlogComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);
}
