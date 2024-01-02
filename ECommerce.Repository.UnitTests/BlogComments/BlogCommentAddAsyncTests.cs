using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task AddAsync_RequiredTextField_ThrowsException()
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
            _ = _blogCommentRepository.AddAsync(blogComment, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task AddAsync_NullValue_ThrowsException()
    {
        // Act
        async Task Action()
        {
            await _blogCommentRepository.AddAsync(null!, CancellationToken);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void AddAsync_AddEntity_ReturnsAddedEntityAndItExistsInDb()
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
        var actual = await _blogCommentRepository.AddAsync(expected, CancellationToken);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        actual.Should().BeEquivalentTo(expected);
        DbContext.BlogComments.Count().Should().Be(1);
        DbContext
            .BlogComments
            .FirstOrDefault()
            .Should()
            .BeEquivalentTo(actual)
            .And
            .BeEquivalentTo(expected);
    }
}
