using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryUpdateTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryUpdateTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

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
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
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
