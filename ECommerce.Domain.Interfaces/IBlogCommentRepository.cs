using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCommentRepository : IRepositoryBase<BlogComment>
{
    Task<PagedList<BlogComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PagedList<BlogComment>> GetAllAcceptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);
}
