using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class ProductCommentRepository(SunflowerECommerceDbContext context) : RepositoryBase<ProductComment>(context),
    IProductCommentRepository
{
    public PagedList<ProductComment> Search(PaginationParameters paginationParameters)
    {
        return PagedList<ProductComment>.ToPagedList(
            context.ProductComments
                .Where(x => x.ProductId != null && x.Name.Contains(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Product),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public PagedList<ProductComment> GetAllAcceptedComments(PaginationParameters paginationParameters)
    {
        return PagedList<ProductComment>.ToPagedList(
            context.ProductComments.Where(x =>
                    x.IsAccepted && x.ProductId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
