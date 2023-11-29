using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentGetByIdTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentGetByIdTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "GetById: Get blogComment by Id")]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        foreach (var blogComment in expected.Values)
        {
            DbContext.BlogComments.Add(blogComment);
        }
        DbContext.SaveChanges();

        // Act
        Dictionary<string, BlogComment?> actual = new();
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(entry.Key, _blogCommentRepository.GetById(entry.Value.Id));
        }

        // Assert
        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
