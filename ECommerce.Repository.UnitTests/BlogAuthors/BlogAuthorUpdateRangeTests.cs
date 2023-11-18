using AutoFixture;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repository;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void UpdateRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.UpdateRange([null!]);
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
            _blogAuthorRepository.UpdateRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        var expected = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(expected);
        DbContext.SaveChanges();

        foreach (var blogAuthor in expected)
        {
            blogAuthor.EnglishName = Guid.NewGuid().ToString();
            blogAuthor.Name = Guid.NewGuid().ToString();
            blogAuthor.Description = Guid.NewGuid().ToString();
        }

        // Act
        _blogAuthorRepository.UpdateRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.BlogAuthors.Should().BeEquivalentTo(expected);
    }
}
