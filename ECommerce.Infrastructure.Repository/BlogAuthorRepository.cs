using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;

public class BlogAuthorRepository : AsyncRepository<BlogAuthor>, IBlogAuthorRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public BlogAuthorRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<BlogAuthor> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.BlogAuthors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<BlogAuthor>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogAuthor>.ToPagedList(
            await _context.BlogAuthors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
