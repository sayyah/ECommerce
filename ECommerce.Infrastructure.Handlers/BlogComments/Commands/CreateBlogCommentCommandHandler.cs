using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogComments.Commands
{
    public class CreateBlogCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) :
        ICommandHandler<CreateBlogCommentCommand, bool>
    {
        private readonly IBlogCommentRepository _blogCommentRepository = unitOfWork.GetRepository<BlogCommentRepository, BlogComment>();
        private BlogComment _blogComment = new();

        public async Task<bool> HandleAsync(CreateBlogCommentCommand command, CancellationToken cancellationToken)
        {
            _blogComment = mapper.Map<BlogComment>(command);
            _blogCommentRepository.Add(_blogComment);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }
}
