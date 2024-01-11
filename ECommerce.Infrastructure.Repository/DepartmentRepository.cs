using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class DepartmentRepository(SunflowerECommerceDbContext context) : RepositoryBase<Department>(context),
    IDepartmentRepository
{
    public async Task<Department?> GetByTitle(string name, CancellationToken cancellationToken)
    {
        return await context.Departments.Where(x => x.Title == name).FirstOrDefaultAsync(cancellationToken);
    }

    public PagedList<Department> Search(PaginationParameters paginationParameters)
    {
        return PagedList<Department>.ToPagedList(
           context.Departments.Where(x => x.Title.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
