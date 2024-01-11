using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Commands
{
    public class EditBlogCommandHandler(IUnitOfWork unitOfWork) :
        ICommandHandler<EditBlogCommand, BlogResult>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<BlogResult> HandleAsync(EditBlogCommand command,
            CancellationToken cancellationToken)
        {
            var repetitiveTitle = await _blogRepository.GetByTitle(command.Title, cancellationToken);
            if (repetitiveTitle != null && repetitiveTitle.Id != command.Id)
                throw new Exception("عنوان مقاله تکراری است");

            if (repetitiveTitle != null) _blogRepository.Detach(repetitiveTitle);
            var repetitiveUrl = await _blogRepository.GetByUrl(command.Url, cancellationToken);
            if (repetitiveUrl != null && repetitiveUrl.Id != command.Id)
                throw new Exception("آدرس مقاله تکراری است");
            
            if (repetitiveUrl != null) _blogRepository.Detach(repetitiveUrl);
            var blog = await _blogRepository.EditWithRelations(command, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);
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
