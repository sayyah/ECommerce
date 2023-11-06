using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IProductUserRankRepository : IAsyncRepository<ProductUserRank>
{
    Task<ProductUserRank?> GetByProductUser(int productId, int userId, CancellationToken cancellationToken);

    Task<double> GetBySumProduct(int productId, CancellationToken cancellationToken);
}
