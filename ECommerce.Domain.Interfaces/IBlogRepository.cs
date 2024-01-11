using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;

namespace ECommerce.Domain.Interfaces;

public interface IBlogRepository : IRepositoryBase<Blog>
{
    Task<IQueryable<Blog>> Search(PaginationParameters paginationParameters);

    Task<IQueryable<Blog>> GetByTagText(PaginationParameters paginationParameters);

    Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken);

    Task<Blog> AddWithRelations(BlogViewModel blogViewModel, CancellationToken cancellationToken);

    Task<Blog> EditWithRelations(BlogViewModel blogViewModel, CancellationToken cancellationToken);

    Task<IQueryable<Blog>> GetWithInclude(int id);

    Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken);
    IQueryable<Blog> GetBlogByIdWithInclude(int blogId);
    IQueryable<Blog> GetBlogByUrlWithInclude(string blogUrl);
}
