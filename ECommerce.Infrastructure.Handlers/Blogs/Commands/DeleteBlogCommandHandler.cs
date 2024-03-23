using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Commands
{
    public class DeleteBlogCommandHandler(IUnitOfWork unitOfWork) :
        ICommandHandler<DeleteBlogCommand, bool>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<bool> HandleAsync(DeleteBlogCommand command,
            CancellationToken cancellationToken)
        {
            await _blogRepository.DeleteById(command.Id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }
}
