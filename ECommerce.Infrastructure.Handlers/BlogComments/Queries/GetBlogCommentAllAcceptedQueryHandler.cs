using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Queries;
using ECommerce.Application.Services.BlogComments.Result;
using ECommerce.Application.Services.Objects;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogComments.Queries
{
    public class GetBlogCommentAllAcceptedQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogCommentQuery, PagedList<BlogCommentResult>>
    {
        private readonly IBlogCommentRepository _blogCommentRepository =
            unitOfWork.GetRepository<BlogCommentRepository, BlogComment>();

        public async Task<PagedList<BlogCommentResult>> HandleAsync(GetBlogCommentQuery query)
        {
            IQueryable<BlogComment> entities = _blogCommentRepository.GetAllAcceptedComments(query.PaginationParameters);
            PagedList<BlogCommentResult> pagedList = PagedList<BlogCommentResult>.ToPagedList(entities.Select(x =>
                    new BlogCommentResult
                    {
                        Id = x.Id,
                        Text = x.Text,
                        Name = x.Name,
                        IsAccepted = x.IsAccepted,
                        BlogTitle = x.Blog!.Title
                    }),
                query.PaginationParameters.PageNumber,
                query.PaginationParameters.PageSize,
                query.PaginationParameters.Search);
            return pagedList;
        }
    }
}
