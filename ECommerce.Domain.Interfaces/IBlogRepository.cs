using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogRepository : IRepositoryBase<Blog>
{
    IQueryable<Blog> Search(PaginationParameters paginationParameters);
    IQueryable<Blog> GetByTagText(string tagText);
    Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken);
    void AddWithRelations(Blog blog);
    void EditWithRelations(Blog blog);
    IQueryable<Blog> GetWithInclude(int id);
    Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken);
    IQueryable<Blog> GetBlogByIdWithInclude(int blogId);
    IQueryable<Blog> GetBlogByUrlWithInclude(string blogUrl);
}
