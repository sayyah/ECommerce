using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void Delete_NullBlog_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Delete_DeleteBlog_EntityNotInRepository()
    {
        // Arrange
        var blogs = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(blogs);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        var blogToDelete = blogs.ElementAt(2);

        // Act
        _blogRepository.Delete(blogToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Blogs.FirstOrDefault(x => x.Id == blogToDelete.Id);

        // Assert
        actual.Should().BeNull();
        DbContext.Blogs.Count().Should().Be(blogs.Count() - 1);
    }
}
