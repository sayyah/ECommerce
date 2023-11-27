using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryGetByIdTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryGetByIdTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetById: Get blogCategory by Id")]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        foreach (var blogCategory in expected.Values)
        {
            DbContext.BlogCategories.Add(blogCategory);
        }
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
