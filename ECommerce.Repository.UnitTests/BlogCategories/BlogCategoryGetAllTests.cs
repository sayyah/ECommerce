using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];
        DbContext.BlogCategories.AddRange(expected.Values);
        DbContext.SaveChanges();

        // Act
        var actuals = await _blogCategoryRepository.GetAll(CancellationToken);

        // Assert
        actuals
            .Should()
            .BeEquivalentTo(
                expected.Values,
                options => options.Excluding(bc => bc.BlogCategories).Excluding(bc => bc.Parent)
            );
    }
}
