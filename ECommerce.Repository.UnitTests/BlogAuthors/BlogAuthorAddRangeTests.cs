using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void AddRange_RequiredNameField_ThrowsException()
    {
        // Arrange
        var blogAuthor = Fixture
            .Build<BlogAuthor>()
            .With(p => p.Name, () => null!)
            .Without(p => p.Blogs)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogAuthorRepository.AddRange(blogAuthor);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_RequiredEnglishNameField_ThrowsException()
    {
        // Arrange
        var blogAuthor = Fixture
            .Build<BlogAuthor>()
            .With(p => p.EnglishName, () => null!)
            .Without(p => p.Blogs)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogAuthorRepository.AddRange(blogAuthor);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.AddRange([ null! ]);
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
            _blogAuthorRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);

        // Act
        _blogAuthorRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.BlogAuthors.Should().BeEquivalentTo(expected);
    }
}
