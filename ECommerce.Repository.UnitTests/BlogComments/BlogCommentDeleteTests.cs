using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task Delete_NullBlogComment_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.Delete(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task Delete_DeleteBlogComment_EntityNotInRepository()
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

        BlogComment blogCommentToDelete = expected.ElementAt(2);

        // Act
        _blogCommentRepository.Delete(blogCommentToDelete);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogComment? actual = DbContext
            .BlogComments
            .FirstOrDefault(x => x.Id == blogCommentToDelete.Id);

        // Assert
        actual.Should().BeNull();
        DbContext.BlogComments.Count().Should().Be(expected.Count() - 1);
    }
}
