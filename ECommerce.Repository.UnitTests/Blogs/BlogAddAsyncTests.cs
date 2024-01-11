using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void AddAsync_RequiredTextField_ThrowsException()
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
            .Create();

        // Act
        async Task Action()
        {
            await _blogRepository.AddAsync(blog, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_RequiredTitleField_ThrowsException()
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
            .Create();

        // Act
        async Task Action()
        {
            await _blogRepository.AddAsync(blog, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_RequiredSummaryField_ThrowsException()
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
            .Create();

        // Act
        async Task Action()
        {
            await _blogRepository.AddAsync(blog, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_RequiredUrlField_ThrowsException()
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
            .Create();

        // Act
        async Task Action()
        {
            await _blogRepository.AddAsync(blog, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_NullValue_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _blogRepository.AddAsync(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntity_AddsEntity()
    {
        // Arrange
        var expected = Fixture.Create<Blog>();

        // Act
        await _blogRepository.AddAsync(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Blogs.FirstOrDefault();

        // Assert
        DbContext.Blogs.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
