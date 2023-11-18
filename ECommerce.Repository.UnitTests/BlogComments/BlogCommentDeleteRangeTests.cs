using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task DeleteRange_NullBlogComment_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.DeleteRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async Task DeleteRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.DeleteRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task DeleteRange_DeleteBlogComments_EntityNotInRepository()
    {
        // Arrange
        IEnumerable<BlogComment> expected = Fixture
            .Build<BlogComment>()
            .Without(p => p.User)
            .Without(p => p.UserId)
            .Without(p => p.Answer)
            .Without(p => p.AnswerId)
            .Without(p => p.Blog)
            .Without(p => p.BlogId)
            .Without(p => p.Employee)
            .Without(p => p.EmployeeId)
            .CreateMany(5);
        DbContext.BlogComments.AddRange(expected);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        BlogComment blogCommentNotToDelete = expected.ElementAt(2);
        IEnumerable<BlogComment> blogCommentsToDelete = expected.Where(
            x => x.Id != blogCommentNotToDelete.Id
        );

        // Act
        _blogCommentRepository.DeleteRange(blogCommentsToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogComments.ToList();

        // Assert
        DbContext.BlogComments.Count().Should().Be(1);
        DbContext.BlogComments.FirstOrDefault().Should().BeEquivalentTo(blogCommentNotToDelete);
    }
}
