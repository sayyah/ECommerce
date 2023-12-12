using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Products;

[Collection("ProductTests")]
public partial class ProductTests : BaseTests
{
    private readonly IProductRepository _productRepository;
    private readonly List<Price> Prices;
    private readonly List<Category> Categories;
    private readonly Dictionary<string, Dictionary<string, Product>> TestSets;

    public ProductTests()
    {
        _productRepository = new ProductRepository(DbContext, HolooDbContext);

        Prices =
        [
            new Price()
            {
                Id = 1,
                ProductId = 1,
                Amount = 10010,
                Exist = 20,
                MaxQuantity = 20,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 2,
                ProductId = 2,
                Amount = 2222,
                Exist = 5,
                MaxQuantity = 5,
                MinQuantity = 1,
            },
            new Price()
            {
                Id = 3,
                ProductId = 3,
                Amount = 92784,
                Exist = 5,
                MaxQuantity = 5,
                MinQuantity = 1,
            },
        ];

        Categories =
        [
            new Category()
            {
                Id = 10,
                Name = "test category 1",
                Depth = 0,
                Path = "test1",
                IsActive = true,
                ParentId = null,
            },
            new Category()
            {
                Id = 20,
                Name = "test category 2",
                Depth = 1,
                Path = "test1/test2",
                IsActive = true,
                ParentId = 10,
            },
            new Category()
            {
                Id = 30,
                Name = "test category 3",
                Depth = 1,
                Path = "test1/test3",
                IsActive = true,
                ParentId = 10,
            },
            new Category()
            {
                Id = 40,
                Name = "test category 4",
                Depth = 1,
                Path = "test1/test4",
                IsActive = false,
                ParentId = 10,
            },
        ];

        TestSets = new()
        {
            ["required_fields"] = new()
            {
                ["test_1"] = new Product
                {
                    Id = 1,
                    MinOrder = 10,
                    Url = "some random-url/w.[]"
                },
                ["test_2"] = new Product
                {
                    Id = 2,
                    Name = "this has name",
                    Url = "some random-url/w.[/\\:D]"
                },
                ["test_3"] = new Product
                {
                    Id = 3,
                    Name = "this also has name",
                    MinOrder = 2
                }
            },
            ["unique_url"] = new()
            {
                ["test_1"] = new Product
                {
                    Id = 1,
                    Name = "this has name",
                    MinOrder = 10,
                    Url = "some random-url/w.[/\\:D]",
                    Prices =  [ Prices[0] ],
                    ProductCategories = new List<Category> { Categories[1] }
                },
                ["test_2"] = new Product
                {
                    Id = 2,
                    Name = "this name",
                    MinOrder = 10,
                    Url = "random-url",
                    Prices =  [ Prices[1] ],
                    ProductCategories = new List<Category> { Categories[2] }
                },
                ["test_3"] = new Product
                {
                    Id = 3,
                    Name = "such product",
                    MinOrder = 69,
                    Url = "much/wow",
                    Prices =  [ Prices[2] ],
                    ProductCategories = new List<Category> { Categories[3] }
                }
            }
        };
    }

    private void AddCategories()
    {
        DbContext.Categories.AddRange(Categories);
        DbContext.SaveChanges();
    }
}
