namespace ECommerce.Infrastructure.Repository;

public class BlogAuthorRepository(SunflowerECommerceDbContext context) : AsyncRepository<BlogAuthor>(context),
    IBlogAuthorRepository
{
    public async Task<BlogAuthor> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.BlogAuthors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<BlogAuthor>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogAuthor>.ToPagedList(
            await context.BlogAuthors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
