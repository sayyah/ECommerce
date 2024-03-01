using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

public class ColorEditeTest : BaseTests
{
    private IColorRepository _colorRepository;
    private readonly CancellationToken _cancellationToken;

    public ColorEditeTest()
    {
        DbContextFake db = new DbContextFake();
        //SunflowerECommerceDbContext dbContext = db.GetDatabaseContext();
        _colorRepository = new ColorRepository(DbContext);
        _cancellationToken = new CancellationToken();
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
        
}
