using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.Blogs;

public partial class BlogTests
{
    [Fact]
    public async void Update_NullInput_ThrowsException()
    {
        // Act
        async Task Action()
        {
            _blogRepository.Update(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Update_UpdateEntity_EntityChanges()
    {
        // Arrange
        var blogs = Fixture.CreateMany<Blog>(5);
        DbContext.Blogs.AddRange(blogs);
        DbContext.SaveChanges();

        var expectedBlog = blogs.ElementAt(2);
        expectedBlog.Title = Fixture.Create<string>();
        expectedBlog.Summary = Fixture.Create<string>();
        expectedBlog.Text = Fixture.Create<string>();
        expectedBlog.Url = Fixture.Create<string>();

        // Act
        _blogRepository.Update(expectedBlog);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.Blogs.Single(p => p.Id == expectedBlog.Id);

        // Assert
        actual.Should().BeEquivalentTo(expectedBlog);
    }
}
