using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogExceptions;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Queries
{
    public class GetBlogByUrlQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogByUrlQuery, BlogResult>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<BlogResult> HandleAsync(GetBlogByUrlQuery query)
        {
            var blog = _blogRepository.GetBlogByUrlWithInclude(query.BlogUrl).FirstOrDefault() ?? throw new NotFoundBlogException(query.BlogUrl);
            var result = new BlogResult
            {
                BlogAuthor = blog.BlogAuthor,
                BlogAuthorId = blog.BlogAuthorId,
                BlogCategoryId = blog.BlogCategoryId,
                CommentCount = blog.BlogComments == null ? 0 : blog.BlogComments.Count(),
                CreateDateTime = blog.CreateDateTime,
                Dislike = blog.Dislike,
                EditDateTime = blog.EditDateTime,
                Id = blog.Id,
                Url = blog.Url,
                TagsId = blog.Tags.Select(x => x.Id).ToList(),
                Tags = blog.Tags.ToList(),
                Image = blog.Image,
                Keywords = blog.Keywords.ToList(),
                KeywordsId = blog.Keywords.Select(x => x.Id).ToList(),
                Like = blog.Like,
                PublishDateTime = blog.PublishDateTime,
                Summary = blog.Summary,
                Text = blog.Text,
                Title = blog.Title,
                Visit = blog.Visit
            };
            return result;
        }
    }
}