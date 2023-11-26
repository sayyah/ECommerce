using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorUpdateTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorUpdateTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "Update: Null input")]
    public void Update_NullInput_ThrowsException()
    {
        // Act
        void actual() => _blogAuthorRepository.Update(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "Update: Update blogAuthor")]
    public void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        Dictionary<string, BlogAuthor> expected = _testSets["simple_tests"];
        DbContext.BlogAuthors.AddRange(expected.Values);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToUpdate = expected.Values.ToArray()[
            Random.Shared.Next(expected.Values.Count)
        ];
        BlogAuthor expectedBlogAuthor = DbContext
            .BlogAuthors
            .Single(p => p.Id == blogAuthorToUpdate.Id)!;
        expectedBlogAuthor.EnglishName = Guid.NewGuid().ToString();
        expectedBlogAuthor.Name = Guid.NewGuid().ToString();
        expectedBlogAuthor.Description = Guid.NewGuid().ToString();

        // Act
        _blogAuthorRepository.Update(expectedBlogAuthor);

        // Assert
        BlogAuthor? actual = DbContext.BlogAuthors.Single(p => p.Id == blogAuthorToUpdate.Id);
        Assert.Equivalent(expectedBlogAuthor, actual);
    }
}
