using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "UpdateRangeAsync: Null BlogCategory")]
    public async Task UpdateRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
       void Actual() => _blogCategoryRepository.UpdateRangeAsync([ null! ], CancellationToken);

        // Assert
        await Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Null Argument")]
    public async Task UpdateRangeAsync_NullArgument_ThrowsException()
    {
        // Act
       void Actual() => _blogCategoryRepository.UpdateRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact(DisplayName = "UpdateRangeAsync: Update blogCategorys")]
    public async void UpdateRangeAsync_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            expected[entry.Key] = DbContext.BlogCategories.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].Name = Guid.NewGuid().ToString();
            expected[entry.Key].Description = Guid.NewGuid().ToString();
        }

        // Act
        await _blogCategoryRepository.UpdateRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogCategories.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
