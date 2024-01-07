namespace ECommerce.Infrastructure.Repository;

public class WishListRepository(SunflowerECommerceDbContext context) : AsyncRepository<WishList>(context),
    IWishListRepository
{
    public Task<WishList> GetByProductUser(int productId, int userId, CancellationToken cancellationToken)
    {
        return context.WishLists
            .Where(x => x.Price.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<WishListViewModel>> GetByIdWithInclude(int userId, CancellationToken cancellationToken)
    {
        return await context.WishLists.Where(x => x.UserId == userId)
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
