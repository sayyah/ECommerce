using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorUpdateRangeTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorUpdateRangeTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateRange: Null BlogAuthor")]
    public void UpdateRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.UpdateRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(actual);
    }

    [Fact(DisplayName = "UpdateRange: Null Argument")]
    public void UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.UpdateRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateRange: Update blogAuthors")]
    public void UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            expected[entry.Key] = DbContext.BlogAuthors.Single(p => p.Id == entry.Value.Id)!;
            expected[entry.Key].EnglishName = Guid.NewGuid().ToString();
            expected[entry.Key].Name = Guid.NewGuid().ToString();
            expected[entry.Key].Description = Guid.NewGuid().ToString();
        }

        // Act
        _blogAuthorRepository.UpdateRange(expected.Values);

        // Assert
        Dictionary<string, BlogAuthor?> actual =  [ ];
        foreach (KeyValuePair<string, BlogAuthor> entry in expected)
        {
            actual.Add(entry.Key, DbContext.BlogAuthors.Single(p => p.Id == entry.Value.Id)!);
        }
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
