using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogExceptions;
using ECommerce.Domain.Exceptions.KeywordExceptions;
using ECommerce.Domain.Exceptions.TagException;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Commands
{
    public class EditBlogCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) :
        ICommandHandler<EditBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();
        private readonly IBlogCategoryRepository _blogCategoryRepository = unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();
        private readonly IBlogAuthorRepository _blogAuthorRepository = unitOfWork.GetRepository<BlogAuthorRepository, BlogAuthor>();
        private readonly IKeywordRepository _keywordRepository = unitOfWork.GetRepository<KeywordRepository, Keyword>();
        private readonly ITagRepository _tagRepository = unitOfWork.GetRepository<TagRepository, Tag>();

        private Blog _blog = new();

        public async Task<bool> HandleAsync(EditBlogCommand command,
            CancellationToken cancellationToken)
        {
            Blog? repetitiveTitle = await GetByTitle(command.Title.Trim(), cancellationToken);
            if (repetitiveTitle != null && repetitiveTitle.Id != command.Id)
                throw new RepetitiveTitleBlogException(command.Title);

            if (repetitiveTitle != null) _blogRepository.Detach(repetitiveTitle);

            Blog? repetitiveUrl = await GetByUrl(command.Url.Trim(), cancellationToken);
            if (repetitiveUrl != null && repetitiveUrl.Id != command.Id)
                throw new RepetitiveAddressBlogException(command.Url);

            Blog? blog = GetBlogById(command.Id);
            if (blog == null)
                throw new NotFoundBlogException(command.Title);

            _blog = mapper.Map<Blog>(command);

            if (repetitiveUrl != null) _blogRepository.Detach(repetitiveUrl);

            if (command.KeywordsId != null)
            {
                await RemoveKeywords(command.KeywordsId);
                await AddKeywords(command.KeywordsId);
            }

            if (command.TagsId != null)
            {
                await RemoveTags(command.TagsId);
                await AddTags(command.TagsId);
            }

            await BlogCategoryGetByIdAsync(command.BlogCategoryId);
            await BlogAuthorGetByIdAsync(command.BlogAuthorId);

            _blogRepository.EditWithRelations(_blog);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }

        private async Task<Blog?> GetByTitle(string title, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetByTitle(title, cancellationToken);
        }
        private async Task<Blog?> GetByUrl(string url, CancellationToken cancellationToken)
        {
            return await _blogRepository.GetByUrl(url, cancellationToken);
        }

        private Blog? GetBlogById(int blogId)
        {
            return _blogRepository.GetByIdWithInclude($"{nameof(Blog.Tags)},{nameof(Blog.Keywords)}", blogId);
        }

        private async Task RemoveKeywords(List<int> keywordsId)
        {
            foreach (var id in keywordsId)
            {
                await _keywordRepository.DeleteById(id, CancellationToken.None);
            }
        }

        private async Task AddKeywords(List<int> keywordsId)
        {
            _blog.Keywords = new List<Keyword>();
            foreach (var id in keywordsId)
            {
                var keyword = await _keywordRepository.GetByIdAsync(CancellationToken.None, id);
                if (keyword == null)
                    throw new NotFoundKeywordException(id);
                _blog.Keywords.Add(keyword);
            }
        }

        private async Task RemoveTags(List<int> tagsId)
        {
            foreach (var id in tagsId)
            {
                await _tagRepository.DeleteById(id, CancellationToken.None);
            }
        }

        private async Task AddTags(List<int> tagsId)
        {
            _blog.Tags = new List<Tag>();
            foreach (var id in tagsId)
            {
                var tag = await _tagRepository.GetByIdAsync(CancellationToken.None, id);
                if (tag == null)
                    throw new NotFoundTagException(id);
                _blog.Tags.Add(tag);
            }
        }
        private async Task BlogCategoryGetByIdAsync(int blogCategoryId)
        {
            _blog.BlogCategory = await _blogCategoryRepository.GetByIdAsync(CancellationToken.None, blogCategoryId);
        }

        private async Task BlogAuthorGetByIdAsync(int blogAuthorId)
        {
            _blog.BlogAuthor = await _blogAuthorRepository.GetByIdAsync(CancellationToken.None, blogAuthorId);
        }
    }
}
