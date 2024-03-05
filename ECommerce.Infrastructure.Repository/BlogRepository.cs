using ECommerce.Application.Services.Objects;

namespace ECommerce.Infrastructure.Repository;

public class BlogRepository(SunflowerECommerceDbContext context) : RepositoryBase<Blog>(context), IBlogRepository
{
    public async Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken)
    {
        return await context.Blogs.Where(x => x.Title == title).FirstOrDefaultAsync(cancellationToken);
    }

    public void AddWithRelations(Blog blog)
    {
        context.Blogs.Add(blog);
    }

    public void EditWithRelations(Blog blog)
    {
        context.Entry(blog).State = EntityState.Modified;
        Entities.Update(blog);
    }

    public IQueryable<Blog> GetWithInclude(int id)
    {
        return context.Blogs.Where(x => x.BlogCategoryId == id)
            .Include(nameof(Blog.BlogAuthor))
            .Include(nameof(Blog.Tags))
            .Include(nameof(Blog.Keywords));
    }

    public async Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken)
    {
        return await context.Blogs.Where(x => x.Url == url).FirstOrDefaultAsync(cancellationToken);
    }

    public IQueryable<Blog> GetBlogByIdWithInclude(int blogId)
    {
        var result = context.Blogs.Where(x => x.Id == blogId)
            .Include(nameof(Blog.BlogAuthor))
            .Include(nameof(Blog.Tags))
            .Include(nameof(Blog.Keywords))
            .Include(nameof(Blog.Image));

        return result;
    }

    public IQueryable<Blog> GetBlogByUrlWithInclude(string blogUrl)
    {
        var result = context.Blogs.Where(x => x.Url == blogUrl)
            .Include(nameof(Blog.BlogAuthor))
            .Include(nameof(Blog.Tags))
            .Include(nameof(Blog.Keywords))
            .Include(nameof(Blog.Image))
            .Include(nameof(Blog.BlogComments));

        return result;
    }

    public async Task<List<Blog>> GetByBlogCategoryId(int categoryId, CancellationToken cancellationToken)
    {
        return await context.Blogs.Where(x => x.BlogCategoryId == categoryId).ToListAsync(cancellationToken);
    }

    public IQueryable<Blog> Search(PaginationParameters paginationParameters)
    {
        return context.Blogs.Where(x => x.Title.Contains(paginationParameters.Search)
                                              && (x.BlogCategoryId == paginationParameters.CategoryId ||
                                                  paginationParameters.CategoryId == 0))
            .Include(x => x.Image)
            .Include(x => x.Keywords)
            .Include(x => x.Tags)
            .Include(x => x.BlogComments)
            .Include(x => x.BlogAuthor).AsNoTracking()
            .OrderBy(on => on.Id);
    }

    public IQueryable<Blog> GetByTagText(string tagText)
    {
        return context.Blogs.Where(x => x.Tags.Any(t => t.TagText == tagText))
                .Include(x => x.Image)
                .Include(x => x.Keywords)
                .Include(x => x.Tags)
                .Include(x => x.BlogAuthor).AsNoTracking()
                .OrderBy(on => on.Id);
    }
}
