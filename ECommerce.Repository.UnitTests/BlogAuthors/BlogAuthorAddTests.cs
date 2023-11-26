using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorAddTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorAddTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "Add: Null value for required Fields")]
    public void Add_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["required_fields"];

        // Act
        Dictionary<string, Action> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(entry.Key, () => _blogAuthorRepository.Add(entry.Value));
        }

        // Assert
        foreach (var action in actual.Values)
        {
            Assert.Throws<DbUpdateException>(action);
        }
    }

    [Fact(DisplayName = "Add: Null BlogAuthor value")]
    public void Add_NullValue_ThrowsException()
    {
        // Act
        Dictionary<string, Action> actual =  [ ];
        void action() => _blogAuthorRepository.Add(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact(DisplayName = "Add: Add BlogAuthor")]
    public void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        foreach (BlogAuthor entry in expected.Values)
        {
            _blogAuthorRepository.Add(entry);
        }

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogAuthors.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
