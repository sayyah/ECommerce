using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

public class ColorDeleteTest : BaseTests
{
    private IColorRepository _colorRepository;
    private readonly CancellationToken _cancellationToken;

    public ColorDeleteTest()
    {
        DbContextFake db = new DbContextFake();
        //SunflowerECommerceDbContext dbContext = db.GetDatabaseContext();
        _colorRepository = new ColorRepository(DbContext);
        _cancellationToken = new CancellationToken();
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
