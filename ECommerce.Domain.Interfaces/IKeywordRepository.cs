using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IKeywordRepository : IRepositoryBase<Keyword>
{
    PagedList<Keyword> Search(PaginationParameters paginationParameters);
    Task<Keyword?> GetByKeywordText(string keywordText, CancellationToken cancellationToken);
    void AddAll(IEnumerable<Keyword> keywords);
    Task<List<Keyword>> GetByProductId(int productId, CancellationToken cancellationToken);
}
