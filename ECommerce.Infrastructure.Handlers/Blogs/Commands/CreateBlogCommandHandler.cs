using ECommerce.Application.Base.Services.Interfaces;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;

namespace ECommerce.Infrastructure.Handlers.Blogs.Commands
{
    public class CreateBlogCommandHandler(IUnitOfWork unitOfWork) :
        ICommandHandler<CreateBlogCommand, BlogResult>
    {
        private readonly IBlogRepository _blogRepository = unitOfWork.GetRepository<BlogRepository, Blog>();

        public async Task<BlogResult> HandleAsync(CreateBlogCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new Exception();
            command.Title = command.Title.Trim();
            var repetitiveTitle = await _blogRepository.GetByTitle(command.Title, cancellationToken);
            if (repetitiveTitle != null)
                throw new Exception("عنوان مقاله تکراری است");
            var repetitiveUrl = await _blogRepository.GetByUrl(command.Url, cancellationToken);
            if (repetitiveUrl != null)
                throw new Exception("آدرس مقاله تکراری است");
            var blog = await _blogRepository.AddWithRelations(command, cancellationToken);
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