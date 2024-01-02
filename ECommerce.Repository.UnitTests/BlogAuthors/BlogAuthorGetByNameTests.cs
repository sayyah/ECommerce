using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        var blogAuthors = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(blogAuthors);
        DbContext.SaveChanges();
        var expected = blogAuthors.ElementAt(2);

        // Act
        var actual = await _blogAuthorRepository.GetByName(expected.Name, CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        var blogAuthors = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(blogAuthors);
        DbContext.SaveChanges();

        // Act
        var actual = await _blogAuthorRepository.GetByName(
            new Guid().ToString(),
            CancellationToken
        );

        // Assert
        actual.Should().BeNull();
    }
}
