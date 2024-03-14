using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Queries;
using ECommerce.Application.Services.BlogComments.Result;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogCommentExeptions;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogComments.Queries
{
    public class GetBlogCommentByIdQueryHandler(IUnitOfWork unitOfWork) :
        IQueryHandler<GetBlogCommentByIdQuery, BlogCommentResult>
    {
        private readonly IBlogCommentRepository _blogCommentRepository = unitOfWork.GetRepository<BlogCommentRepository, BlogComment>();

        public async Task<BlogCommentResult> HandleAsync(GetBlogCommentByIdQuery query)
        {
            var blogComment = _blogCommentRepository.GetByIdAsync(CancellationToken.None, query.Id).Result;
            if (blogComment != null)
            {
                var result = new BlogCommentResult()
                {
                    Id = blogComment.Id,
                    Name = blogComment.Name,
                    Text = blogComment.Text,
                    IsAccepted = blogComment.IsAccepted,
                    BlogTitle = blogComment.Blog?.Title
                };
                return result;
            }
            else
                throw new NotFoundBlogCommentException(query.Id);
        }
    }
}
