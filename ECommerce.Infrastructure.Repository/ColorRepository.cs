<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class ColorRepository : AsyncRepository<Color>, IColorRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ColorRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Color> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Colors.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Color>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Color>.ToPagedList(
            await _context.Colors.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
