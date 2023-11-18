using AutoFixture;
using ECommerce.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public void GetAll_GetAllAddedEntities_EntityExistsInRepository()
    {
        // Arrange
        BlogCategory root = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .With(p => p.Blogs, () => [])
            .Create();
        BlogCategory child1 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [])
            .Create();
        BlogCategory child2 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [])
            .Create();
        root.BlogCategories!.Add(child1);
        root.BlogCategories.Add(child2);
        List<BlogCategory> expected = [root, child1, child2];
        DbContext.BlogCategories.AddRange(expected);
        DbContext.SaveChanges();

        // Act
        var actuals = _blogCategoryRepository.GetAll("");

        // Assert
        actuals.Should().BeEquivalentTo(expected).And.HaveCount(3);
    }
}
