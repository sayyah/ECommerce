using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void Add_RequiredTextField_ThrowsException()
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
            _blogRepository.Add(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_RequiredTitleField_ThrowsException()
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
            _blogRepository.Add(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_RequiredSummaryField_ThrowsException()
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
            _blogRepository.Add(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_RequiredUrlField_ThrowsException()
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
            _blogRepository.Add(blog);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_NullValue_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.Add(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.Create<Blog>();

        // Act
        _blogRepository.Add(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Blogs.FirstOrDefault();

        // Assert
        DbContext.Blogs.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
