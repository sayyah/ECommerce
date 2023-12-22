using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

public partial class BlogCommentTests
{
    [Fact]
    public void AddRange_RequiredFields_ThrowsException()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["required_fields"];

        // Act
        void Actual() => _blogCommentRepository.AddRange(expected.Values);

        // Assert
        Assert.Throws<DbUpdateException>(Actual);
    }

    [Fact]
    public void AddRange_NullBlogComment_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.AddRange([ null! ]);

        // Assert
        Assert.Throws<NullReferenceException>(Actual);
    }

    [Fact]
    public void AddRange_NullArgument_ThrowsException()
    {
        // Act
        void Actual() => _blogCommentRepository.AddRange(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(Actual);
    }

    [Fact]
    public void AddRange_AddEntities_EntityExistsInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];

        // Act
        _blogCommentRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }

    [Fact]
    public void AddRange_NoSave_EntityNotInRepository()
    {
        // Arrange
        Dictionary<string, BlogComment> expected = TestSets["simple_tests"];

        // Act
        _blogCommentRepository.AddRange(expected.Values);

        // Assert
        Dictionary<string, BlogComment?> actual =  [ ];
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        foreach (var item in actual.Values)
        {
            Assert.Null(item);
        }

        DbContext.SaveChanges();
        actual.Clear();
        foreach (KeyValuePair<string, BlogComment> entry in expected)
        {
            actual.Add(
                entry.Key,
                DbContext.BlogComments.FirstOrDefault(x => x.Id == entry.Value.Id)
            );
        }

        actual.Values.Should().BeEquivalentTo(expected.Values);
    }
}
