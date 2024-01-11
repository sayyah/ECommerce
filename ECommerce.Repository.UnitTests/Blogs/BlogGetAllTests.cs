using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        var expected = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(expected);
        DbContext.SaveChanges();

        // Act
        var actuals = _blogRepository.GetAll("");

        // Assert
        actuals.Should().BeEquivalentTo(expected);
    }
}
