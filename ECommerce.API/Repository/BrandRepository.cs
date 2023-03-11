using ECommerce.API.DataContext;
using ECommerce.API.Interface;
using ECommerce.API.Utilities;
using Ecommerce.Entities;
using Ecommerce.Entities.Helper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repository;

public class BrandRepository : AsyncRepository<Brand>, IBrandRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public BrandRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagedList<Brand>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Brand>.ToPagedList(
            await _context.Brands.Where(x => x.Name.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }

    public async Task<Brand> GetByName(string name, CancellationToken cancellationToken)
    {
        return await _context.Brands.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<int>> GetByIdList(int brandId, CancellationToken cancellationToken)
    {
        var brandsId = new List<int>();

        var brands = _context.Brands.Where(x => x.Id == brandId);
        if (brands.Any())
        {
            foreach (var i in brands.Select(x => x.Id))
            {
                brandsId.Add(i);
            }
        }
        else
        {
            brandsId.Add(brandId);
        }

        return brandsId;
    }
}