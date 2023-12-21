using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async Task Update_NullInput_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogCommentRepository.Update(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Update_UpdateEntity_EntityChanges()
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

        BlogComment blogCommentToUpdate = expected.ElementAt(2);
        BlogComment expectedBlogComment = DbContext
            .BlogComments
            .Single(p => p.Id == blogCommentToUpdate.Id);
        expectedBlogComment.Text = Fixture.Create<string>();
        expectedBlogComment.Email = Fixture.Create<string>();
        expectedBlogComment.Name = Fixture.Create<string>();

        // Act
        _blogCommentRepository.Update(expectedBlogComment);
        await UnitOfWork.SaveAsync(CancellationToken);
        BlogComment? actual = DbContext.BlogComments.Single(p => p.Id == blogCommentToUpdate.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedBlogComment);
    }
}
