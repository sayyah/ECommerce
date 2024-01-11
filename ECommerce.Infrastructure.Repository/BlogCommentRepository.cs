using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class BlogCommentRepository(SunflowerECommerceDbContext context) : RepositoryBase<BlogComment>(context),
    IBlogCommentRepository
{
    public PagedList<BlogComment> Search(PaginationParameters paginationParameters)
    {
        return PagedList<BlogComment>.ToPagedList(
            context.BlogComments.Where(x => x.Name.Contains(paginationParameters.Search) && x.BlogId != null)
                .Include(x => x.Blog).AsNoTracking()
                .OrderByDescending(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public PagedList<BlogComment> GetAllAcceptedComments(PaginationParameters paginationParameters)
    {
        return PagedList<BlogComment>.ToPagedList(
            context.BlogComments.Where(x =>
                    x.IsAccepted && x.BlogId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
