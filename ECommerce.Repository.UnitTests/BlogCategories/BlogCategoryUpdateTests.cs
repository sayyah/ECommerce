using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "Update: Null input")]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void actual() => _blogCategoryRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "Update: Update blogCategory")]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogCategory blogCategoryToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
        BlogCategory expectedBlogCategory = DbContext
            .BlogCategories
            .Single(p => p.Id == blogCategoryToUpdate.Id)!;
        expectedBlogCategory.Name = Guid.NewGuid().ToString();
        expectedBlogCategory.Description = Guid.NewGuid().ToString();

        // Act
        _blogCategoryRepository.Update(expectedBlogCategory);

        // Assert
        BlogCategory? actual = DbContext
            .BlogCategories
            .Single(p => p.Id == blogCategoryToUpdate.Id);
        Assert.Equivalent(expectedBlogCategory, actual);
    }
}
