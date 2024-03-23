using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IPriceRepository : IRepositoryBase<Price>
{
    PagedList<Price> Search(PaginationParameters paginationParameters);
    void AddAll(IEnumerable<Price> prices);

    void EditAll(IEnumerable<Price> prices, int id);

    Task<List<Price>?> PriceOfProduct(int id, CancellationToken cancellationToken);

    Task<List<ProductIndexPageViewModel?>> TopDiscounts(int count, CancellationToken cancellationToken);
    List<int> GetProductIdWithsArticleCodeCustomer(string articleCodeCustomer);
}
