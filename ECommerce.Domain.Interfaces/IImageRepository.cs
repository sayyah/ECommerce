using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IImageRepository : IRepositoryBase<Image>
{
    PagedList<Image> Search(PaginationParameters paginationParameters);
    Task DeleteByName(string name, CancellationToken cancellationToken);
    Task<List<Image>?> GetByProductId(int productId, CancellationToken cancellationToken);
    Task<Image?> GetByBlogId(int blogId, CancellationToken cancellationToken);
}
