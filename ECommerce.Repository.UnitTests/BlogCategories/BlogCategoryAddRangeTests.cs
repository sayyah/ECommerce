using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogCategories;

public partial class BlogCategoryTests
{
    [Fact]
    public async Task AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["required_fields"];

        // Act
        void Actual() => _blogCategoryRepository.AddRange(expected.Values);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact]
    public async Task AddRange_NullBlogCategory_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.AddRange(new List<BlogCategory>());
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public async Task AddRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCategoryRepository.AddRange(null!);
        await UnitOfWork.SaveAsync(CancellationToken);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];

        // Act
        _blogCategoryRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogCategory> expected = TestSets["simple_tests"];

        // Act
        _blogCategoryRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, BlogCategory?> actual =  [ ];
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, BlogCategory> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogCategories.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
