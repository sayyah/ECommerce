using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

public class ColorOthereTest : BaseTests
{
    private IColorRepository _colorRepository;
    private readonly CancellationToken _cancellationToken;

    public ColorOthereTest()
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

}
