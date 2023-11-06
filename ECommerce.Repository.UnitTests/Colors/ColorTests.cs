using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

<<<<<<< HEAD
[CollectionDefinition("ColorTests", DisableParallelization = true)]
[Collection("ColorTests")]
=======
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
public class ColorTests : BaseTests
{
    private readonly IColorRepository _colorRepository;

    public ColorTests()
    {
        _colorRepository = new ColorRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_AddNewEntity_ReturnsSameEntity()
    {
        //Arrange
        var id = 2;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };

        //Act
        var newColor = await _colorRepository.AddAsync(color, CancellationToken);

        //Assert
        Assert.Equal(id, newColor.Id);
        Assert.Equal(name, newColor.Name);
        Assert.Equal(colorCode, newColor.ColorCode);
    }

    [Fact]
    public async Task GetAll_CountAllEntities_ReturnsTwoEntities()
    {
        //Arrange
        var id = 3;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };
        await DbContext.Colors.AddAsync(color, CancellationToken);
        await DbContext.SaveChangesAsync(CancellationToken);

        //Act
        var newColor = await _colorRepository.GetAll(CancellationToken);

        //Assert
        Assert.Equal(2, newColor.Count());
    }
}