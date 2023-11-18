using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

public partial class BlogAuthorTests
{
    [Fact]
    public async void DeleteRange_NullBlogAuthor_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.DeleteRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async void DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogAuthorRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void DeleteRange_DeleteBlogAuthors_EntityNotInRepository()
    {
        // Arrange
        var blogAuthors = Fixture.Build<BlogAuthor>().Without(p => p.Blogs).CreateMany(5);
        DbContext.BlogAuthors.AddRange(blogAuthors);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogAuthor blogAuthorNotToDelete = blogAuthors.ElementAt(2);
        IEnumerable<BlogAuthor> blogAuthorsToDelete = blogAuthors.Where(
            x => x.Id != blogAuthorNotToDelete.Id
        );

        // Act
        _blogAuthorRepository.DeleteRange(blogAuthorsToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        DbContext.BlogAuthors.Count().Should().Be(1);
        DbContext.BlogAuthors.FirstOrDefault().Should().BeEquivalentTo(blogAuthorNotToDelete);
    }
}
