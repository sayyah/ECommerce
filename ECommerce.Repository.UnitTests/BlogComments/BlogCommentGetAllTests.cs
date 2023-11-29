using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[Collection("BlogComments")]
public class BlogCommentGetAllTests : BaseTests
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentGetAllTests()
    {
        _blogCommentRepository = new BlogCommentRepository(DbContext);
    }

    [Fact(DisplayName = "GetAll: Get all blogComment")]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = BlogCommentTestUtils.TestSets["simple_tests"];
        foreach (var blogComment in expected.Values)
        {
            DbContext.BlogComments.Add(blogComment);
        }
        DbContext.SaveChanges();

        // Act
        var actuals = _blogCommentRepository.GetAll(CancellationToken).Result;

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options =>
                    options
                        .Excluding(bc => bc.Answer)
                        .Excluding(bc => bc.User)
                        .Excluding(bc => bc.Blog)
                        .Excluding(bc => bc.Employee)
            );
    }
}
