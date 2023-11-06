<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class SizeRepository : AsyncRepository<Size>, ISizeRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public SizeRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Size> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Sizes.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Size>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Size>.ToPagedList(
            await _context.Sizes.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
