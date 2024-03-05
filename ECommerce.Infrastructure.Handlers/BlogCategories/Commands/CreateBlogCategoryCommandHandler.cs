using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogExceptions;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Commands;

    public class CreateBlogCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) : 
        ICommandHandler<CreateBlogCategoryCommand, bool>
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository = unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();
        private BlogCategory _blogCategory = new();

    public async Task<bool> HandleAsync(CreateBlogCategoryCommand command, CancellationToken cancellationToken)
    {
        command.Name = command.Name!.Trim();
        var repetitiveCategory = await GetByName(command);
        if (repetitiveCategory != null) return false;
        _blogCategory = mapper.Map<BlogCategory>(command);
        if (command.BlogsId != null) { await AddBlogs(command.BlogsId); }
        _blogCategoryRepository.Add(_blogCategory);
        await unitOfWork.SaveAsync(cancellationToken);
        return true;
    }

    private async Task<BlogCategory?> GetByName(CreateBlogCategoryCommand command)
    {
       return await _blogCategoryRepository.GetByName(command.Name!, command.ParentId, CancellationToken.None);
    }

    private async Task AddBlogs(ICollection<int> blogsId)
    {
        _blogCategory.Blogs = new List<Blog>();
        foreach (var id in blogsId)
        {
            var blog = await _blogRepository.GetByIdAsync(CancellationToken.None, id);
            if (blog == null)
                throw new NotFoundBlogException(id.ToString());
            _blogCategory.Blogs.Add(blog);
        }
    }
}
