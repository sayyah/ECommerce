using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Interfaces;
using ECommerce.Application.Services.Objects;
using ECommerce.Application.ViewModels;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Helper;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Queries
{
    public class GetCurrencyAccountsQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogsQuery, PagedList<BlogViewModel>>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<PagedList<BlogViewModel>> HandleAsync(GetBlogsQuery query)
        {
            IQueryable<Blog> entities = await _blogRepository.Search(query.PaginationParameters);
            PagedList<BlogViewModel> pagedList = PagedList<BlogViewModel>.ToPagedList(entities.Select(x => new BlogViewModel
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
