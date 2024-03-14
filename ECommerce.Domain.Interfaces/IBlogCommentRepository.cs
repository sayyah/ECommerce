using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogCommentRepository : IRepositoryBase<BlogComment>
{
    IQueryable<BlogComment> Search(PaginationParameters paginationParameters);

    IQueryable<BlogComment> GetAllAcceptedComments(PaginationParameters paginationParameters);
}
