using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class BlogAuthorRepository(SunflowerECommerceDbContext context) : RepositoryBase<BlogAuthor>(context),
    IBlogAuthorRepository
{
    public async Task<BlogAuthor> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.BlogAuthors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<BlogAuthor> Search(PaginationParameters paginationParameters)
    {
        return PagedList<BlogAuthor>.ToPagedList(
            context.BlogAuthors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
