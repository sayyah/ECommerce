using ECommerce.Domain.Entities;
using Xunit;

namespace ECommerce.Repository.UnitTests.BlogAuthors;

[CollectionDefinition("BlogAuthors", DisableParallelization = true)]
public class BlogAuthorTestUtils
{
    public static Dictionary<string, Dictionary<string, BlogAuthor>> TestSets =>
        new()
        {
            ["required_fields"] = new()
            {
                ["no_name"] = new()
                {
                    Id = 1,
                    EnglishName = "NAME :D",
                    Description = "test description"
                },
                ["no_english_name"] = new() { Id = 2, Name = "جاسم", },
            },
            ["simple_tests"] = new()
            {
                ["test_1"] = new()
                {
                    Id = 3,
                    Name = "اسم",
                    EnglishName = "NAME :D",
                    Description = "test description"
                },
                ["test_2"] = new()
                {
                    Id = 4,
                    Name = "جاسم",
                    EnglishName = "gasem :))",
                    Description = "jasem gasem"
                },
                ["test_3"] = new()
                {
                    Id = 5,
                    Name = "آقای هاشمی",
                    EnglishName = "Hashemi?",
                    Description = "Hashemi nadarim inja..."
                },
            },
        };
}
