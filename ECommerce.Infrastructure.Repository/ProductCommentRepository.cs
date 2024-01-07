namespace ECommerce.Infrastructure.Repository;

public class ProductCommentRepository(SunflowerECommerceDbContext context) : AsyncRepository<ProductComment>(context),
    IProductCommentRepository
{
    public async Task<PagedList<ProductComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductComment>.ToPagedList(
            await context.ProductComments
                .Where(x => x.ProductId != null && x.Name.Contains(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Product).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<PagedList<ProductComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductComment>.ToPagedList(
            await context.ProductComments.Where(x =>
                    x.IsAccepted && x.ProductId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
