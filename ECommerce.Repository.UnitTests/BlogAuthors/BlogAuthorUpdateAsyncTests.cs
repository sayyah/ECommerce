using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[Collection("BlogAuthors")]
public class BlogAuthorUpdateAsyncTests : BaseTests
{
    private readonly IBlogAuthorRepository _blogAuthorRepository;
    private readonly Dictionary<string, Dictionary<string, BlogAuthor>> _testSets =
        BlogAuthorTestUtils.TestSets;

    public BlogAuthorUpdateAsyncTests()
    {
        _blogAuthorRepository = new BlogAuthorRepository(DbContext);
    }

    [Fact(DisplayName = "UpdateAsync: Null input")]
    public void UpdateAsync_NullInput_ThrowsException()
    {
        // Act
        Task<BlogAuthor> actual() => _blogAuthorRepository.UpdateAsync(null!, CancellationToken);

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(actual);
    }

    [Fact(DisplayName = "UpdateAsync: Update blogAuthor")]
    public async void UpdateAsync_UpdateEntity_EntityChanges()
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
        await _blogAuthorRepository.UpdateAsync(expectedBlogAuthor, CancellationToken);

        // Assert
        BlogAuthor? actual = DbContext.BlogAuthors.Single(p => p.Id == blogAuthorToUpdate.Id);
        Assert.Equivalent(expectedBlogAuthor, actual);
    }
}
