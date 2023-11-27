using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryGetByNameTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryGetByNameTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetByName: Get blogCategory by Name")]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        foreach (var blogCategory in expected.Values)
        {
            DbContext.BlogCategories.Add(blogCategory);
        }
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

    [Fact(DisplayName = "GetByName: Non existing name")]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        foreach (var blogCategory in expected.Values)
        {
            DbContext.BlogCategories.Add(blogCategory);
        }
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
