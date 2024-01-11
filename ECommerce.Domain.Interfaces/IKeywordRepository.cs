using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IKeywordRepository : IRepositoryBase<Keyword>
{
    Task<PagedList<Keyword>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Keyword?> GetByKeywordText(string keywordText, CancellationToken cancellationToken);
    void AddAll(IEnumerable<Keyword> keywords);
    Task<List<Keyword>> GetByProductId(int productId, CancellationToken cancellationToken);
}
