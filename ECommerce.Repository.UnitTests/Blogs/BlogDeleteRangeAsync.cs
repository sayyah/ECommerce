using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void DeleteRange_NullBlog_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.DeleteRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void DeleteRange_DeleteBlogs_EntityNotInRepository()
    {
        // Arrange
        var blogs = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(blogs);
        DbContext.SaveChanges();

        var blogNotToDelete = blogs.ElementAt(2);
        var blogsToDelete = blogs.Where(x => x.Id != blogNotToDelete.Id);

        // Act
        _blogRepository.DeleteRange(blogsToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Blogs.Count().Should().Be(1);
        DbContext.Blogs.FirstOrDefault().Should().BeEquivalentTo(blogNotToDelete);
    }
}
