using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact(DisplayName = "GetById: Get blogComment by Id")]
    public void GetById_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        DbContext.BlogComments.AddRange(TestSets["simple_tests"].Values);
        DbContext.SaveChanges();

        BlogComment expected = TestSets["simple_tests"]["test_2"];

        // Act
        var actual = _blogCommentRepository.GetById(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
