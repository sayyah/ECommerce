<<<<<<< HEAD
﻿using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Infrastructure.Repository;
=======
﻿namespace ECommerce.Infrastructure.Repository;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

public class ProductUserRankRepository : AsyncRepository<ProductUserRank>, IProductUserRankRepository
{
    private readonly SunflowerECommerceDbContext _context;

    public ProductUserRankRepository(SunflowerECommerceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductUserRank?> GetByProductUser(int productId, int userId, CancellationToken cancellationToken)
    {
        return await _context.ProductUserRanks
            .Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<double> GetBySumProduct(int productId, CancellationToken cancellationToken)
    {
        var sum = await _context.ProductUserRanks
            .Where(x => x.ProductId == productId).SumAsync(s => s.Stars, cancellationToken);

        var count = await _context.ProductUserRanks
            .Where(x => x.ProductId == productId).CountAsync(cancellationToken);

        return count == 0 ? 0 : (double)sum / count;
    }
}
