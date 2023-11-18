using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task AddRangeAsync_RequiredFields_ThrowsException()
    {
        // Arrange
        IEnumerable<BlogComment> blogComment = Fixture
            .Build<BlogComment>()
            .With(p => p.Text, () => null!)
            .Without(p => p.User)
            .Without(p => p.UserId)
            .Without(p => p.Answer)
            .Without(p => p.AnswerId)
            .Without(p => p.Blog)
            .Without(p => p.BlogId)
            .Without(p => p.Employee)
            .Without(p => p.EmployeeId)
            .CreateMany(1);

        // Act
        async Task Action()
        {
            _blogCommentRepository.AddRange(blogComment);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task AddRangeAsync_NullBlogComment_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.AddRange([null!]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async Task AddRangeAsync_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.AddRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task AddRangeAsync_AddEntities_EntityExistsInRepository()
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

        // Act
        _blogCommentRepository.AddRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogComments.ToList();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
