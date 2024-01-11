using AutoMapper;
using ECommerce.Application.Services.Blogs.Commands;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions.BlogExceptions;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Handlers.Blogs.Commands;
using ECommerce.Infrastructure.Repository;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Handler.UnitTests.Blogs
{
    public class CreateBlogCommandHandlerTests : CreateBlogCommandBaseTests
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public CreateBlogCommandHandlerTests()
        {
            _blogRepository = new BlogRepository(DbContext);
            _mapper = Substitute.For<IMapper>();
        }

        [Fact]
        public async Task HandleAsync_CreateBlogTitle_ReturnTrue()
        {
            //Arrange
            AddTags(1);
            AddKeyword(1);
            CreateBlogCommand command = new CreateBlogCommand
            {
                TagsId = [1],
                BlogCategoryId = 1,
                Title = "ali",
                BlogAuthorId = 2,
                CreateDateTime = DateTime.Now,
                KeywordsId = [1],
                PublishDateTime = DateTime.Now,
                Summary = "Ali2",
                Text = "AliIsOk",
                Url = "A-l-i"
            };
            var handler = new CreateBlogCommandHandler(_mapper, UnitOfWork);

            //Act
            bool result = await handler.HandleAsync(command, CancellationToken);

            //Assert
            result.ShouldBe(true);
        }

        [Fact]
        public async Task HandleAsync_CreateDuplicateBlogTitle_ThrowsException()
        {
            //Arrange
            Tag tag = AddTags(1);
            Keyword keyword = AddKeyword(1);
            Blog blog = new Blog
            {
                BlogCategoryId = 1,
                Title = "ali",
                BlogAuthorId = 2,
                CreateDateTime = DateTime.Now,
                PublishDateTime = DateTime.Now,
                Summary = "Ali2",
                Text = "AliIsOk",
                Url = "A-l-i",
                Tags = [tag],
                Keywords = [keyword]
            };
            DbContext.Blogs.Add(blog);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            var command = new CreateBlogCommand
            {
                TagsId = [1],
                BlogCategoryId = 11,
                Title = "ali",
                BlogAuthorId = 1,
                CreateDateTime = DateTime.Now,
                KeywordsId = [1],
                PublishDateTime = DateTime.Now,
                Summary = "Ali2",
                Text = "AliIsOk",
                Url = "A-l-i-2"
            };
            var handler = new CreateBlogCommandHandler(_mapper, UnitOfWork);

            //Act
            bool result = await handler.HandleAsync(command, CancellationToken);

            //Assert
            result.ShouldNotBe(true);
            result.ShouldBeOfType<RepetitiveTitleBlogException>();
        }
    }
}

