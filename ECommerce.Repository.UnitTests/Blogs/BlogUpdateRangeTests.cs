using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void UpdateRange_NullBlog_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.UpdateRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.UpdateRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        var expected = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(expected);
        DbContext.SaveChanges();

        foreach (var blog in expected)
        {
            blog.Title = Fixture.Create<string>();
            blog.Summary = Fixture.Create<string>();
            blog.Text = Fixture.Create<string>();
            blog.Url = Fixture.Create<string>();
        }

        // Act
        _blogRepository.UpdateRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.Blogs.Should().BeEquivalentTo(expected);
    }
}
