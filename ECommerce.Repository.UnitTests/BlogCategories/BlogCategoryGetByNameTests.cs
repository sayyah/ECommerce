using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogCategoryRepository.GetByName(
                    entry.Value.Name,
                    entry.Value.ParentId,
                    CancellationToken
                )
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogCategoryRepository.GetByName(
                    new Guid().ToString(),
                    entry.Value.ParentId,
                    CancellationToken
                )
            );
        }

        // Assert
        actual.Values.Should().AllSatisfy(x => x.Should().BeNull());
    }
}
