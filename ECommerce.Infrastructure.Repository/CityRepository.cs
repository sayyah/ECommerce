<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class CityRepository : AsyncRepository<City>, ICityRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public CityRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<City> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Cities.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<City>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<City>.ToPagedList(
            await _context.Cities.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
