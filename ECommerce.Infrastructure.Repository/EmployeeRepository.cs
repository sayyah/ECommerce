using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class EmployeeRepository(SunflowerECommerceDbContext context) : RepositoryBase<Employee>(context),
    IEmployeeRepository
{
    public async Task<Employee?> GetByName(string name, CancellationToken cancellationToken)
    {
        return await context.Employees.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Employee> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Employee>.ToPagedList(
            context.Employees.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
