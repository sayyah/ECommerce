using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        var blogs = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(blogs);
        DbContext.SaveChanges();
        var expected = blogs.ElementAt(2);

        // Act
        var actual = await _blogRepository.GetByIdAsync(CancellationToken, expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
