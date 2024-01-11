using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task UpdateRange_NullBlogComment_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.UpdateRange([ null! ]);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<NullReferenceException>(Action);
    }

    [Fact]
    public async Task UpdateRange_NullArgument_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.UpdateRange(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async Task UpdateRange_UpdateEntities_EntitiesChange()
    {
        // Arrange
        var expected = Fixture
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

        foreach (BlogComment blogComment in expected)
        {
            blogComment.Text = Fixture.Create<string>();
            blogComment.Email = Fixture.Create<string>();
            blogComment.Name = Fixture.Create<string>();
        }

        // Act
        _blogCommentRepository.UpdateRange(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogComments.ToList();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
