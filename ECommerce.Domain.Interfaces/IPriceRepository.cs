using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IPriceRepository : IRepositoryBase<Price>
{
    Task<PagedList<Price>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    void AddAll(IEnumerable<Price> prices);

    void EditAll(IEnumerable<Price> prices, int id);

    Task<List<Price>?> PriceOfProduct(int id, CancellationToken cancellationToken);

    Task<List<ProductIndexPageViewModel?>> TopDiscounts(int count, CancellationToken cancellationToken);
    List<int> GetProductIdWithsArticleCodeCustomer(string articleCodeCustomer);
}
