using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IKeywordRepository : IAsyncRepository<Keyword>
{
    Task<PagedList<Keyword>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Keyword> GetByKeywordText(string keywordText, CancellationToken cancellationToken);
    Task<int> AddAll(IEnumerable<Keyword> keywords, CancellationToken cancellationToken);
    Task<List<Keyword>> GetByProductId(int productId, CancellationToken cancellationToken);
}
