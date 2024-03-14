using AutoMapper;
using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogComments.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogComments.Commands
{
    public class DeleteBlogCommentCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) :
        ICommandHandler<DeleteBlogCommentCommand, bool>
    {
        private readonly IBlogCommentRepository _blogCommentRepository = unitOfWork.GetRepository<BlogCommentRepository, BlogComment>();
        private BlogComment _blogComment = new();

        public async Task<bool> HandleAsync(DeleteBlogCommentCommand command, CancellationToken cancellationToken)
        {
            _blogComment = mapper.Map<BlogComment>(command);

            await _blogCommentRepository.DeleteById(command.Id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }
}
