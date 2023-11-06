<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class StateRepository : AsyncRepository<State>, IStateRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public StateRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<State> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.States.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<State>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<State>.ToPagedList(
            await _context.States.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Name).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
