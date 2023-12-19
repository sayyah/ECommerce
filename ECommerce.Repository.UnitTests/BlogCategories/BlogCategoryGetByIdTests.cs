using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogCategory?> actual = new();
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(entry.Key, _blogCategoryRepository.GetById(entry.Value.Id));
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
