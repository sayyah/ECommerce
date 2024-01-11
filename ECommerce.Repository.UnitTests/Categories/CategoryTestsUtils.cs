using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace Ecommerce.Repository.UnitTests.Categories;

[Collection("CategoryTests")]
public partial class CategoryTests : BaseTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryTests()
    {
        _categoryRepository = new CategoryRepository(DbContext);
    }
}
