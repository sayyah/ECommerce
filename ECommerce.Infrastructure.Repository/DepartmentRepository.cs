namespace ECommerce.Infrastructure.Repository;

public class DepartmentRepository(SunflowerECommerceDbContext context) : RepositoryBase<Department>(context),
    IDepartmentRepository
{
    public async Task<Department?> GetByTitle(string name, CancellationToken cancellationToken)
    {
        return await context.Departments.Where(x => x.Title == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Department>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Department>.ToPagedList(
            await context.Departments.Where(x => x.Title.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
