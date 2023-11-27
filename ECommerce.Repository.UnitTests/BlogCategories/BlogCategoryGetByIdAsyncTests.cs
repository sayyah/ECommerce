using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

[Collection("BlogCategories")]
public class BlogCategoryGetByIdAsyncTests : BaseTests
{
    private readonly IBlogCategoryRepository _blogCategoryRepository;
    private readonly Dictionary<string, Dictionary<string, BlogCategory>> _testSets =
        BlogCategoryTestUtils.TestSets;

    public BlogCategoryGetByIdAsyncTests()
    {
        _blogCategoryRepository = new BlogCategoryRepository(DbContext);
    }

    [Fact(DisplayName = "GetByIdAsync: Get blogCategory by Id")]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = _testSets["simple_tests"];
        foreach (var blogCategory in expected.Values)
        {
            DbContext.BlogCategories.Add(blogCategory);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogCategory> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                await _blogCategoryRepository.GetByIdAsync(CancellationToken, entry.Value.Id)
            );
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
