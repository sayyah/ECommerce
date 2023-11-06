using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface ITagRepository : IAsyncRepository<Tag>
{
    Task<PagedList<Tag>> Search(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Tag> GetByTagText(string tagText, CancellationToken cancellationToken);
    Task<List<TagProductId>> GetByProductId(int productId, CancellationToken cancellationToken);
    Task<List<Tag>> GetAllProductTags(CancellationToken cancellationToken);
    Task<List<Tag>> GetAllBlogTags(CancellationToken cancellationToken);
}
