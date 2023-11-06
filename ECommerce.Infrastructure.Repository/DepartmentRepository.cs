namespace ECommerce.Infrastructure.Repository;

public class DepartmentRepository : AsyncRepository<Department>, IDepartmentRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public DepartmentRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Department> GetByTitle(string name, CancellationToken cancellationToken)
    {
        return await _context.Departments.Where(x => x.Title == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Department>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Department>.ToPagedList(
            await _context.Departments.Where(x => x.Title.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
