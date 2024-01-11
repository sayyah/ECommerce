using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void Add_RequiredNameField_ThrowsException()
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
            _blogAuthorRepository.Add(blogAuthor);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async void Add_RequiredEnglishNameField_ThrowsException()
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
            _blogAuthorRepository.Add(blogAuthor);
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
            _blogAuthorRepository.Add(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).Create();

        // Act
        _blogAuthorRepository.Add(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogAuthors.FirstOrDefault();

        // Assert
        DbContext.BlogAuthors.Count().Should().Be(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
