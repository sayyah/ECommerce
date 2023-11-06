<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class BlogCommentRepository : AsyncRepository<BlogComment>, IBlogCommentRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public BlogCommentRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<BlogComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogComment>.ToPagedList(
            await _context.BlogComments.Where(x => x.Name.Contains(paginationParameters.Search) && x.BlogId != null)
                .Include(x => x.Blog).AsNoTracking()
                .OrderByDescending(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<PagedList<BlogComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<BlogComment>.ToPagedList(
            await _context.BlogComments.Where(x =>
                    x.IsAccepted && x.BlogId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
