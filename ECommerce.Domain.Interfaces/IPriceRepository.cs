using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IPriceRepository : IAsyncRepository<Price>
{
    Task<PagedList<Price>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<int> AddAll(IEnumerable<Price> prices, CancellationToken cancellationToken);

    Task<int> EditAll(IEnumerable<Price> prices, int id, CancellationToken cancellationToken);

    Task<IEnumerable<Price>> PriceOfProduct(int id, CancellationToken cancellationToken);

    Task<List<ProductIndexPageViewModel?>> TopDiscounts(int count, CancellationToken cancellationToken);
    List<int> GetProductIdWithsArticleCodeCustomer(string articleCodeCustomer);
}
