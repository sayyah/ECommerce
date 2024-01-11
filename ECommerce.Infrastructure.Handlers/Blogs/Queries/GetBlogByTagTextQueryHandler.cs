using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Application.Services.Objects;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Queries
{
    public class GetBlogByTagTextQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogByTagTextQuery, PagedList<BlogResult>>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<PagedList<BlogResult>> HandleAsync(GetBlogByTagTextQuery query)
        {
            IQueryable<Blog> entities = _blogRepository.GetByTagText(query.TagText);
            PagedList<BlogResult> pagedList = PagedList<BlogResult>.ToPagedList(entities.Select(x => new BlogResult
            {
                BlogAuthor = x.BlogAuthor,
                BlogAuthorId = x.BlogAuthorId,
                BlogCategoryId = x.BlogCategoryId,
                CommentCount = x.BlogComments == null ? 0 : x.BlogComments.Count(),
                CreateDateTime = x.CreateDateTime,
                Dislike = x.Dislike,
                EditDateTime = x.EditDateTime,
                Id = x.Id,
                Url = x.Url,
                TagsId = x.Tags.Select(x => x.Id).ToList(),
                Tags = x.Tags.ToList(),
                Image = x.Image,
                Keywords = x.Keywords.ToList(),
                KeywordsId = x.Keywords.Select(x => x.Id).ToList(),
                Like = x.Like,
                PublishDateTime = x.PublishDateTime,
                Summary = x.Summary,
                Text = x.Text,
                Title = x.Title,
                Visit = x.Visit
            }),
                query.PaginationParameters.PageNumber,
                query.PaginationParameters.PageSize,
                query.PaginationParameters.Search);
            return pagedList;
        }
    }
}
