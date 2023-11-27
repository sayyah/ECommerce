using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryUpdateRangeTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryUpdateRangeTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRange: Null BlogCategory")]
    public void UpdateRange_NullBlogCategory_ThrowsException()
    {
        // Act
        void actual() => _blogCategoryRepository.UpdateRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRange: Null Argument")]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogCategoryRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRange: Update blogCategorys")]
    public void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
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
        _blogCategoryRepository.UpdateRange(expected.Values);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogCategories.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
