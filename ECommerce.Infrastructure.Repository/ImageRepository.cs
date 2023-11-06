<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class ImageRepository : AsyncRepository<Image>, IImageRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ImageRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> DeleteByName(string name, CancellationToken cancellationToken)
    {
        var image = await _context.Images.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
        _context.Images.Remove(image);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Image>> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await
            _context.Images.Where(x => x.ProductId == productId).ToListAsync(cancellationToken);
    }

    public async Task<Image> GetByBlogId(int blogId, CancellationToken cancellationToken)
    {
        return await _context.Images.Where(x => x.BlogId == blogId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Image>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Image>.ToPagedList(
            await _context.Images.Where(x => x.Alt.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
