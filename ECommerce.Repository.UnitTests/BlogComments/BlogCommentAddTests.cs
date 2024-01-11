using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task Add_RequiredFields_ThrowsException()
    {
        // Arrange
        BlogComment blogComment = Fixture
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
            .Create();

        // Act
        async Task Action()
        {
            _blogCommentRepository.Add(blogComment);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task Add_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.Add(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        BlogComment expected = Fixture
            .Build<BlogComment>()
            .Without(p => p.User)
            .Without(p => p.UserId)
            .Without(p => p.Answer)
            .Without(p => p.AnswerId)
            .Without(p => p.Blog)
            .Without(p => p.BlogId)
            .Without(p => p.Employee)
            .Without(p => p.EmployeeId)
            .Create();

        // Act
        _blogCommentRepository.Add(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogComments.FirstOrDefault(x => x.Id == expected.Id);

        // Assert
        DbContext.BlogComments.Should().HaveCount(1);
        actual.Should().BeEquivalentTo(expected);
    }
}
