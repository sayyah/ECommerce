namespace ECommerce.Infrastructure.Repository;

public class BlogCommentRepository(SunflowerECommerceDbContext context) : AsyncRepository<BlogComment>(context),
    IBlogCommentRepository
{
    public async Task<PagedList<BlogComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogComment>.ToPagedList(
            await context.BlogComments.Where(x => x.Name.Contains(paginationParameters.Search) && x.BlogId != null)
                .Include(x => x.Blog).AsNoTracking()
                .OrderByDescending(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<PagedList<BlogComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogComment>.ToPagedList(
            await context.BlogComments.Where(x =>
                    x.IsAccepted && x.BlogId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
