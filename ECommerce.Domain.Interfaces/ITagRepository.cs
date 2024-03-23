using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface ITagRepository : IRepositoryBase<Tag>
{
    PagedList<Tag> Search(PaginationParameters paginationParameters);
    Task<Tag?> GetByTagText(string tagText, CancellationToken cancellationToken);
    Task<List<TagProductId>> GetByProductId(int productId, CancellationToken cancellationToken);
    Task<List<Tag>> GetAllProductTags(CancellationToken cancellationToken);
    Task<List<Tag>> GetAllBlogTags(CancellationToken cancellationToken);
}
