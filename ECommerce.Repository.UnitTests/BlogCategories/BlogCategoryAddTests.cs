using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact(DisplayName = "Add: Null value for required Fields")]
    public void Add_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["required_fields"];

        // Act
        Dictionary<string, Action> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(entry.Key, () => _blogCategoryRepository.Add(entry.Value));
        }

        // Assert
        foreach (var action in actual.Values)
        {
            Assert.Throws<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "Add: Null BlogCategory value")]
    public void Add_NullValue_ThrowsException()
    {
        // Act
        void action() => _blogCategoryRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Add: Add BlogCategory")]
    public void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        BlogCategory expected = TestSets["simple_tests"]["test_1"];

        // Act
        _blogCategoryRepository.Add(expected);

        // Assert
        var actual = DbContext.BlogCategories.FirstOrDefault(x => x.Id == expected.Id);

        actual.Should().BeEquivalentTo(expected);
    }
}
