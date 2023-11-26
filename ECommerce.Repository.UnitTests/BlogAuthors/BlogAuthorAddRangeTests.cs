using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorAddRangeTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorAddRangeTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "AddRange: Null value for required Fields")]
    public void AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["required_fields"];

        // Act
        void actual() => _blogAuthorRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null BlogAuthor")]
    public void AddRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.AddRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "AddRange: Null argument")]
    public void AddRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "AddRange: Add BlogAuthors all together")]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        _blogAuthorRepository.AddRange(expected.Values);

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

    [Fact(DisplayName = "AddRange: No save")]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];

        // Act
        _blogAuthorRepository.AddRange(expected.Values, false);

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogAuthors.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
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
