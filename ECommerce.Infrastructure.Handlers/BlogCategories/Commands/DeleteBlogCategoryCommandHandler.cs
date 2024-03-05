using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.BlogCategories.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.BlogCategories.Commands;

    public class DeleteBlogCategoryCommandHandler(IUnitOfWork unitOfWork) :
        ICommandHandler<DeleteBlogCategoryCommand, bool>
    {
        private readonly IBlogCategoryRepository _blogCategoryRepository =
            unitOfWork.GetRepository<BlogCategoryRepository, BlogCategory>();

        public async Task<bool> HandleAsync(DeleteBlogCategoryCommand command, CancellationToken cancellationToken)
        {
            await _blogCategoryRepository.DeleteById(command.Id, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
            return true;
        }
    }

