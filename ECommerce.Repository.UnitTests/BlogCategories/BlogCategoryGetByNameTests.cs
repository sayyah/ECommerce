using AutoFixture;
using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async void GetByName_GetAddedEntityByName_EntityExistsInRepository()
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
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        BlogCategory child2 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        root.BlogCategories!.Add(child1);
        root.BlogCategories.Add(child2);
        List<BlogCategory> list =  [ root, child1, child2 ];
        DbContext.BlogCategories.AddRange(list);
        DbContext.SaveChanges();

        var expected = child2;

        // Act
        BlogCategory? actual = await _blogCategoryRepository.GetByName(
            expected.Name,
            expected.ParentId,
            CancellationToken
        );

        // Assert
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async void GetByName_GetAddedEntityByNonExistingName_ReturnsNull()
    {
        // Arrange
        BlogCategory root = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .With(p => p.Blogs, () => [ ])
            .Create();
        BlogCategory child1 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        BlogCategory child2 = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => [ ])
            .With(p => p.Parent, () => root)
            .With(p => p.ParentId, () => root.Id)
            .With(p => p.Blogs, () => [ ])
            .Create();
        root.BlogCategories!.Add(child1);
        root.BlogCategories.Add(child2);
        List<BlogCategory> list =  [ root, child1, child2 ];
        DbContext.BlogCategories.AddRange(list);
        DbContext.SaveChanges();

        // Act
        BlogCategory? actual = await _blogCategoryRepository.GetByName(
            new Guid().ToString(),
            null,
            CancellationToken
        );

        // Assert
        Assert.Null(actual);
    }
}
