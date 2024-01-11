namespace ECommerce.Infrastructure.Repository;

public class ProductUserRankRepository(SunflowerECommerceDbContext context) : RepositoryBase<ProductUserRank>(context),
    IProductUserRankRepository
{
    public async Task<ProductUserRank?> GetByProductUser(int productId, int userId, CancellationToken cancellationToken)
    {
        return await context.ProductUserRanks
            .Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<double> GetBySumProduct(int productId, CancellationToken cancellationToken)
    {
        var sum = await context.ProductUserRanks
            .Where(x => x.ProductId == productId).SumAsync(s => s.Stars, cancellationToken);

        var count = await context.ProductUserRanks
            .Where(x => x.ProductId == productId).CountAsync(cancellationToken);

        return count == 0 ? 0 : (double)sum / count;
    }
}
