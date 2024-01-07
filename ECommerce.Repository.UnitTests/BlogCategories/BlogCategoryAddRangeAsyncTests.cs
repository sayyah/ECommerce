using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "AddRangeAsync: Null value for required Fields")]
    public async Task AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["required_fields"];

        // Act
        Task actual() => _blogCategoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null BlogCategory")]
    public async Task AddRangeAsync_NullBlogCategory_ThrowsException()
    {
        // Act
        async Task actual() =>
            await _blogCategoryRepository.AddRangeAsync([ null! ], CancellationToken);

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Null argument")]
    public async Task AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        Task actual() => _blogCategoryRepository.AddRangeAsync(null!, CancellationToken);

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRangeAsync: Add BlogCategorys all together")]
    public async void AddRangeAsync_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];

        // Act
        await _blogCategoryRepository.AddRangeAsync(expected.Values, CancellationToken);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact(DisplayName = "AddRangeAsync: No save")]
    public async void AddRangeAsync_NoSave_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];

        // Act
        await _blogCategoryRepository.AddRangeAsync(expected.Values, CancellationToken, false);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
