<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class ProductCommentRepository : AsyncRepository<ProductComment>, IProductCommentRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ProductCommentRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<ProductComment>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductComment>.ToPagedList(
            await _context.ProductComments
                .Where(x => x.ProductId != null && x.Name.Contains(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Product).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<PagedList<ProductComment>> GetAllAccesptedComments(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<ProductComment>.ToPagedList(
            await _context.ProductComments.Where(x =>
                    x.IsAccepted && x.ProductId == Convert.ToInt32(paginationParameters.Search))
                .AsNoTracking().OrderByDescending(on => on.Id).Include(x => x.Answer).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
