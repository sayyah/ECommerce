using AutoFixture;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async Task Add_RequiredNameField_ThrowsException()
    {
        // Arrange
        BlogCategory blogCategory = Fixture
            .Build<BlogCategory>()
            .With(p => p.Name, () => null!)
            .With(p => p.BlogCategories, () => [])
            .With(p => p.Parent, () => null!)
            .With(p => p.Blogs, () => [])
            .Create();

        // Act
        async Task Action()
        {
            _blogCategoryRepository.Add(blogCategory);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(Action);
    }

    [Fact]
    public async Task Add_NullValue_ThrowsExceptionAsync()
    {
        // Act
        async Task Action()
        {
            _blogCategoryRepository.Add(null!);
            await UnitOfWork.SaveAsync(CancellationToken);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(Action);
    }

    [Fact]
    public async void Add_AddEntity_EntityExistsInRepository()
    {
        // Arrange
        BlogCategory expected = Fixture
            .Build<BlogCategory>()
            .With(p => p.BlogCategories, () => null)
            .With(p => p.Parent, () => null)
            .With(p => p.ParentId, () => null)
            .With(p => p.Blogs, () => [])
            .Create();

        // Act
        _blogCategoryRepository.Add(expected);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actual = DbContext.BlogCategories.FirstOrDefault(x => x.Id == expected.Id);

        // Assert
        Assert.Equal(1, DbContext.BlogCategories.Count());
        Assert.Equivalent(expected, actual);
    }
}
