using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void AddRange_RequiredTextField_ThrowsException()
    {
        // Arrange
        var blog = Fixture
            .Build<Blog>()
            .With(p => p.Text, () => null!)
            .Without(p => p.BlogAuthor)
            .Without(p => p.BlogAuthorId)
            .Without(p => p.BlogCategory)
            .Without(p => p.BlogCategoryId)
            .Without(p => p.BlogComments)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.Image)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogRepository.AddRange(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredTitleField_ThrowsException()
    {
        // Arrange
        var blog = Fixture
            .Build<Blog>()
            .With(p => p.Title, () => null!)
            .Without(p => p.BlogAuthor)
            .Without(p => p.BlogAuthorId)
            .Without(p => p.BlogCategory)
            .Without(p => p.BlogCategoryId)
            .Without(p => p.BlogComments)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.Image)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogRepository.AddRange(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredSummaryField_ThrowsException()
    {
        // Arrange
        var blog = Fixture
            .Build<Blog>()
            .With(p => p.Summary, () => null!)
            .Without(p => p.BlogAuthor)
            .Without(p => p.BlogAuthorId)
            .Without(p => p.BlogCategory)
            .Without(p => p.BlogCategoryId)
            .Without(p => p.BlogComments)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.Image)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogRepository.AddRange(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredUrlField_ThrowsException()
    {
        // Arrange
        var blog = Fixture
            .Build<Blog>()
            .With(p => p.Url, () => null!)
            .Without(p => p.BlogAuthor)
            .Without(p => p.BlogAuthorId)
            .Without(p => p.BlogCategory)
            .Without(p => p.BlogCategoryId)
            .Without(p => p.BlogComments)
            .Without(p => p.Keywords)
            .Without(p => p.Tags)
            .Without(p => p.Image)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogRepository.AddRange(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_NullBlog_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.AddRange([ null! ]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void AddRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.CreateMany<Blog>(5);

        // Act
        _blogRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Blogs.Should().BeEquivalentTo(expected);
    }
}
