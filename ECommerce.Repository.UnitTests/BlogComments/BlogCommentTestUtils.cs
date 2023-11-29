using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogComments;

[CollectionDefinition("BlogComments", DisableParallelization = true)]
public class BlogCommentTestUtils
{
    public static readonly Dictionary<string, Dictionary<string, BlogComment>> TestSets =
        new()
        {
            ["required_fields"] = new()
            {
                ["no_text"] = new()
                {
                    Id = 1,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = false,
                    IsAnswered = true, // interesting
                    Email = "invalid email :D", // doesn't matter to database of course
                    Name = "Batman",
                },
            },
            ["simple_tests"] = new()
            {
                ["test_1"] = new()
                {
                    Id = 2,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = false,
                    IsAnswered = true,
                    Email = "invalid email :D",
                    Name = "Batman",
                    Text = Guid.NewGuid().ToString(),
                },
                ["test_2"] = new()
                {
                    Id = 3,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = true,
                    IsAnswered = true,
                    Email = "Tony@Stark.com",
                    Name = "Iron-Man",
                    Text = Guid.NewGuid().ToString(),
                },
                ["test_3"] = new()
                {
                    Id = 4,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = false,
                    IsAnswered = false,
                    Email = "test2@test.com",
                    Name = "Hulk",
                    Text = Guid.NewGuid().ToString(),
                },
                ["test_4"] = new()
                {
                    Id = 5,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = true,
                    IsAnswered = false,
                    Email = "Tony@Stark.com",
                    Name = "Iron-Man",
                    Text = Guid.NewGuid().ToString(),
                },
                ["test_5"] = new()
                {
                    Id = 6,
                    IsAccepted = true,
                    DateTime = DateTime.Now,
                    IsRead = false,
                    IsAnswered = false,
                    Email = "test3@tester.com",
                    Name = "Darth Vader",
                    Text = Guid.NewGuid().ToString(),
                },
            },
        };
}
