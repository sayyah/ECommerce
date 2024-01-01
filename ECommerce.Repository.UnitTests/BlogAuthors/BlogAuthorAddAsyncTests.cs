using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void AddAsync_RequiredNameField_ThrowsException()
    {
        // Arrange
        var blogAuthor = Fixture
            .Build<BlogAuthor>()
            .With(p => p.Name, () => null!)
            .Without(p => p.Blogs)
            .Create();

        // Act
        async Task Action()
        {
            await _blogAuthorRepository.AddAsync(blogAuthor, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void AddAsync_RequiredEnglishNameField_ThrowsException()
    {
        // Arrange
        var blogAuthor = Fixture
            .Build<BlogAuthor>()
            .With(p => p.EnglishName, () => null!)
            .Without(p => p.Blogs)
            .Create();

        // Act
        async Task Action()
        {
            await _blogAuthorRepository.AddAsync(blogAuthor, CancellationToken);
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
            await _blogAuthorRepository.AddAsync(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntities()
    {
        // Arrange
        var expected = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).Create();

        // Act
        await _blogAuthorRepository.AddAsync(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogAuthors.FirstOrDefault();

        // Assert
        DbContext.BlogAuthors.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
