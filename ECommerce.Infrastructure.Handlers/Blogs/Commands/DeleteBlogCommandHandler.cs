using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Commands
{
    public class DeleteBlogCommandHandler(IUnitOfWork unitOfWork) :
        ICommandHandler<DeleteBlogCommand, BlogResult>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<BlogResult> HandleAsync(DeleteBlogCommand command,
            CancellationToken cancellationToken)
        {
            await _blogRepository.DeleteById(command.Id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            return null;
        }
    }
}
