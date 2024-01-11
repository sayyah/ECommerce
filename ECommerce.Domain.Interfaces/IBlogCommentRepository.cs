using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCommentRepository : IRepositoryBase<BlogComment>
{
    PagedList<BlogComment> Search(PaginationParameters paginationParameters);

    PagedList<BlogComment> GetAllAcceptedComments(PaginationParameters paginationParameters);
}
