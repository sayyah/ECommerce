<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class EmployeeRepository : AsyncRepository<Employee>, IEmployeeRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public EmployeeRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Employee> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Employees.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Employee>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Employee>.ToPagedList(
            await _context.Employees.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
