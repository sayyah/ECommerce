using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogRepository : IRepositoryBase<Blog>
{
    IQueryable<Blog> Search(PaginationParameters paginationParameters);

    IQueryable<Blog> GetByTagText(PaginationParameters paginationParameters);

    Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken);

    Task<Blog> AddWithRelations(BlogResult blogViewModel, CancellationToken cancellationToken);

    Task<Blog> EditWithRelations(BlogResult blogViewModel, CancellationToken cancellationToken);

    IQueryable<Blog> GetWithInclude(int id);

    Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken);
    IQueryable<Blog> GetBlogByIdWithInclude(int blogId);
    IQueryable<Blog> GetBlogByUrlWithInclude(string blogUrl);
}
