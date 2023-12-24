using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void Delete_NullBlogAuthor_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Delete_DeleteBlogAuthor_EntityNotInRepository()
    {
        // Arrange
        var blogAuthors = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(blogAuthors);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorToDelete = blogAuthors.ElementAt(2);

        // Act
        _blogAuthorRepository.Delete(blogAuthorToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogAuthor? actual = DbContext
            .BlogAuthors
            .FirstOrDefault(x => x.Id == blogAuthorToDelete.Id);

        // Assert
        actual.Should().BeNull();
        DbContext.BlogAuthors.Count().Should().Be(blogAuthors.Count() - 1);
    }
}
