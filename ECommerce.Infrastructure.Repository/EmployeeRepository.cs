namespace ECommerce.Infrastructure.Repository;

public class EmployeeRepository(SunflowerECommerceDbContext context) : RepositoryBase<Employee>(context),
    IEmployeeRepository
{
    public async Task<Employee?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Employees.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Employee>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Employee>.ToPagedList(
            await context.Employees.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
