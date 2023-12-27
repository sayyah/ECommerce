using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

public class ColorTests : BaseTests
{
    private IColorRepository _colorRepository;
    private readonly CancellationToken _cancellationToken;

    public ColorTests()
    {
        DbContextFake db = new DbContextFake();
        //SunflowerECommerceDbContext dbContext = db.GetDatabaseContext();
        _colorRepository = new ColorRepository(DbContext);
        _cancellationToken = new CancellationToken();
    }


    [Fact]
    public async Task GetAllAsync_CountAllEntities_ReturnsTwoEntities()
    {
        //Arrange
        var id = 1000;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };
        DbContext.Colors.Add(color);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        //Act
        var newColor = await _colorRepository.GetAllAsync(CancellationToken);

        //Assert
        Assert.Equal(2, newColor.Count());
    }

    [Fact]
    public async Task Add_AddNewEntity_ReturnsSameEntity()
    {
        //Arrange
        var id = 1001;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var expectedColor = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };

        //Act
        _colorRepository.Add(expectedColor);
        await UnitOfWork.SaveAsync(CancellationToken);
        Color actualColor = DbContext.Colors.Where(c => c.Id == id).First();

        //Assert
        Assert.Equal(expectedColor.Id, actualColor.Id);
        Assert.Equal(expectedColor.Name, actualColor.Name);
        Assert.Equal(expectedColor.ColorCode, actualColor.ColorCode);
    }

    [Fact]
    public void Add_AddNullEntity_ReturnsException()
    {
        //Arrange
        Color expectedColor = new();

        //Act

        //Assert
        Assert.Throws<DbUpdateException>(() => _colorRepository.Add(expectedColor));
    }

    [Fact]
    public async Task Edit_EditColorName_ReturnsApdateColorNameect()
    {
        //Arrange
        var id = 1002;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };
        DbContext.Colors.Add(color);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();
        color.Name = Guid.NewGuid().ToString();

        //Act
        _colorRepository.Add(color);
        await UnitOfWork.SaveAsync(CancellationToken); 
        var actualColor = DbContext.Colors.Where(c => c.Id == id).First();

        //Assert
        Assert.Equal(color.Name, actualColor.Name);
    }

    [Fact]
    public async Task Delete_DeleteEntity_ReturnsNull()
    {
        //Arrange
        var id = 1003;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };
        DbContext.Colors.Add(color);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        //Act
        _colorRepository.Delete(color);
        await UnitOfWork.SaveAsync(CancellationToken);
        var actualColor = DbContext.Colors.Where(c => c.Id == id).FirstOrDefault();

        //Assert
        Assert.Null(actualColor);
    }

    [Fact]
    public async Task Delete_DeleteEntity_ReturnsZeroCount()
    {
        //Arrange
        int expectedCount = 0;
        var id = 1004;
        var name = Guid.NewGuid().ToString();
        var colorCode = Guid.NewGuid().ToString();
        var color = new Color
        {
            Id = id,
            Name = name,
            ColorCode = colorCode
        };
        DbContext.Colors.Add(color);
        DbContext.SaveChanges();
        DbContext.ChangeTracker.Clear();

        //Act
        _colorRepository.Delete(color);
        await UnitOfWork.SaveAsync(CancellationToken);
        int actualColor = DbContext.Colors.Count();

        //Assert
        Assert.Equal(expectedCount, actualColor);
    }

}