﻿namespace ECommerce.Infrastructure.Repository;

public class WishListRepository : AsyncRepository<WishList>, IWishListRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public WishListRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<WishList> GetByProductUser(int productId, int userId, CancellationToken cancellationToken)
    {
        return _context.WishLists
            .Where(x => x.Price.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<WishListViewModel>> GetByIdWithInclude(int userId, CancellationToken cancellationToken)
    {
        return await _context.WishLists.Where(x => x.UserId == userId)
            .Include(x => x.Price.Product)
            .Include(x => x.Price.Product.Brand)
            .Include(x => x.Price.Product.Images)
            .Select(p => new WishListViewModel
            {
                Id = p.Id,
                ProductId = p.Price.ProductId,
                Url = p.Price.Product.Url,
                Name = p.Price.Product.Name,
                Price = p.Price,
                ImagePath =
                    $"{p.Price.Product.Images.FirstOrDefault().Path}/{p.Price.Product.Images.FirstOrDefault().Name}",
                Brand = p.Price.Product.Brand.Name,
                Alt = p.Price.Product.Images.FirstOrDefault().Alt
            })
            .ToListAsync(cancellationToken);
    }
}