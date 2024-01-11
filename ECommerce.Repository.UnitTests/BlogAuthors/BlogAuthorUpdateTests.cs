using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void Update_NullInput_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.Update(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        var blogAuthors = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(blogAuthors);
        DbContext.SaveChanges();

        BlogAuthor expectedBlogAuthor = blogAuthors.ElementAt(2);
        expectedBlogAuthor.EnglishName = Guid.NewGuid().ToString();
        expectedBlogAuthor.Name = Guid.NewGuid().ToString();
        expectedBlogAuthor.Description = Guid.NewGuid().ToString();

        // Act
        _blogAuthorRepository.Update(expectedBlogAuthor);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogAuthor? actual = DbContext.BlogAuthors.Single(p => p.Id == expectedBlogAuthor.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedBlogAuthor);
    }
}
