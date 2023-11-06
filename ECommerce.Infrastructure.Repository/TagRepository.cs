<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class TagRepository : AsyncRepository<Tag>, ITagRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public TagRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Tag> GetByTagText(string tagText, CancellationToken cancellationToken)
    {
        return await _context.Tags.Where(x => x.TagText == tagText).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TagProductId>> GetByProductId(int productId, CancellationToken cancellationToken)
    {
        return await _context.Tags.Where(x => x.Products.Any(y => y.Id == productId))
            .Select(x => new TagProductId { Id = x.Id, ProductsId = x.Products.Select(x => x.Id) })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Tag>> GetAllProductTags(CancellationToken cancellationToken)
    {
        var result = await _context.Tags.Where(x => x.Products.Any()).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<List<Tag>> GetAllBlogTags(CancellationToken cancellationToken)
    {
        var result = await _context.Tags.Where(x => x.Blogs.Any()).ToListAsync(cancellationToken);
        return result;
    }

    public async Task<PagedList<Tag>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        return PagedList<Tag>.ToPagedList(
            await _context.Tags.Where(x => x.TagText.Contains(paginationParameters.Search)).AsNoTracking()
                .OrderBy(on => on.Id).ToListAsync(cancellationToken),
            paginationParameters.PageNumber,
            paginationParameters.PageSize);
    }
}
