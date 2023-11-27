using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryAddRangeTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryAddRangeTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "AddRange: Null value for required Fields")]
    public void AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["required_fields"];

        // Act
        void actual() => _blogCategoryRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null BlogCategory")]
    public void AddRange_NullBlogCategory_ThrowsException()
    {
        // Act
        void actual() => _blogCategoryRepository.AddRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null argument")]
    public void AddRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogCategoryRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRange: Add BlogCategories all together")]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];

        // Act
        _blogCategoryRepository.AddRange(expected.Values);

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

    [Fact(DisplayName = "AddRange: No save")]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];

        // Act
        _blogCategoryRepository.AddRange(expected.Values, false);

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
