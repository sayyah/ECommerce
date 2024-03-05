using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogCategoryExceptions;
using ECommerce.Domain.Exceptions.BlogExceptions;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Commands;

    public class EditBlogCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) :
        ICommandHandler<EditBlogCategoryCommand, bool>
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository = unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        private BlogCategory _blogCategory = new();

        public async Task<bool> HandleAsync(EditBlogCategoryCommand command, 
            CancellationToken cancellationToken)
        {
            command.Name = command.Name!.Trim();
            BlogCategory? repetitiveName = await GetByName(command.Name, cancellationToken);
            if (repetitiveName != null && repetitiveName.Id != command.Id)
                throw new RepetitiveNameBlogCategoryException(command.Name);
            if (repetitiveName != null) _blogCategoryRepository.Detach(repetitiveName);
            
            BlogCategory? blogCategory = await GetBlogCategoryById(command.Id);
            if (blogCategory == null)
                throw new NotFoundBlogCategoryException(command.Name);
            _blogCategoryRepository.Detach(blogCategory);

            command.Parent = await GetParentByParentId(command.ParentId);
            
            _blogCategory = mapper.Map<BlogCategory>(command);

            if (command.BlogsId != null)
            {
                await RemoveCategoryIdFromBlogs(command.Id);
                await AddCategoryIdToBlogs(command.BlogsId, command.Id);
            }

            _blogCategoryRepository.EditWithRelations(_blogCategory);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }

        private async Task<EditBlogCategoryCommand?> GetParentByParentId(int? id)
    {
        var result = await _blogCategoryRepository.GetByIdAsync(CancellationToken.None, id);
        return mapper.Map<EditBlogCategoryCommand>(result);
    }

    private async Task AddCategoryIdToBlogs(ICollection<int> commandBlogsId, int blogCategoryId)
        {
            _blogCategory.Blogs = new List<Blog>();
            foreach (var id in commandBlogsId)
            {
                var blog = await _blogRepository.GetByIdAsync(CancellationToken.None, id);
                if (blog == null)
                    throw new NotFoundBlogException(id.ToString());
                blog.BlogCategoryId = blogCategoryId;
                _blogCategory.Blogs.Add(blog);
            }
        }

        private async Task RemoveCategoryIdFromBlogs(int blogCategoryId)
        {
            var blogs = await _blogRepository.GetByBlogCategoryId(blogCategoryId, CancellationToken.None);
            foreach (var item in blogs)
            {
                item.BlogCategoryId = blogCategoryId;
            }
            _blogRepository.UpdateRange(blogs);
        }
        
        private async Task<BlogCategory?> GetBlogCategoryById(int id)
        {
            return await _blogCategoryRepository.GetByIdsAsync(CancellationToken.None, id);
        }

        private async Task<BlogCategory?> GetByName(string name, CancellationToken cancellationToken)
        {
            return await _blogCategoryRepository.GetByName(name,null, cancellationToken);
        }
}
