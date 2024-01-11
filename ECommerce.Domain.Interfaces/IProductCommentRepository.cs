using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductCommentRepository : IRepositoryBase<ProductComment>
{
    PagedList<ProductComment> Search(PaginationParameters paginationParameters);

    PagedList<ProductComment> GetAllAcceptedComments(PaginationParameters paginationParameters);
}
