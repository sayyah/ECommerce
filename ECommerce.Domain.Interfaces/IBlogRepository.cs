using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces.Utilities;

namespace ECommerce.Domain.Interfaces;

public interface IBlogRepository : IRepositoryBase<Blog>
{
    Task<PagedList<BlogViewModel>> Search(PaginationParameters paginationParameters,
        CancellationToken cancellationToken);

    Task<PagedList<Blog>> GetByTagText(PaginationParameters paginationParameters, CancellationToken cancellationToken);
    Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken);

    Task<Blog> AddWithRelations(BlogViewModel blogViewModel);

    Task<Blog> EditWithRelations(BlogViewModel blogViewModel, CancellationToken cancellationToken);

    Task<IEnumerable<Blog>> GetWithInclude(int id, CancellationToken cancellationToken);

    Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken);
    IQueryable<Blog> GetBlogByIdWithInclude(int blogId);
    IQueryable<Blog> GetBlogByUrlWithInclude(string blogUrl);
}
