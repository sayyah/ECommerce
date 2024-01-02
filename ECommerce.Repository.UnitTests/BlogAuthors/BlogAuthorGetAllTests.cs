using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(expected);
        DbContext.SaveChanges();

        // Act
        var actuals = _blogAuthorRepository.GetAll("");

        // Assert
        actuals.Should().BeEquivalentTo(expected);
    }
}
