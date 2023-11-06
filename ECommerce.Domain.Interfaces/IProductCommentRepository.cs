using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IProductCommentRepository : IAsyncRepository<ProductComment>
{
    Task<PagedList<ProductComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PagedList<ProductComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);
}
