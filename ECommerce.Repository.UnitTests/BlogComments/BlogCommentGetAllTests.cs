using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "GetAll: Get all blogComment")]
    public async void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];
        DbContext.BlogComments.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        var actuals = await _blogCommentRepository.GetAll(CancellationToken);

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
