using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public async void GetByIdAsync_GetAddedEntityById_EntityExistsInRepository()
    {
        // Arrange
        DbContext.BlogComments.AddRange(TestSets["simple_tests"].Values);
        DbContext.SaveChanges();

        BlogComment expected = TestSets["simple_tests"]["test_2"];

        // Act
        var actual = await _blogCommentRepository.GetByIdAsync(CancellationToken, expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
