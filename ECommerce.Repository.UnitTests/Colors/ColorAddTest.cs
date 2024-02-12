using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using ECommerce.Repository.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using Xunit;

namespace ECommerce.Repository.UnitTests.Colors;

public class ColorAddTest : BaseTests
{
    private IColorRepository _colorRepository;
    private readonly CancellationToken _cancellationToken;

    public ColorAddTest()
    {
        DbContextFake db = new DbContextFake();
        //SunflowerECommerceDbContext dbContext = db.GetDatabaseContext();
        _colorRepository = new ColorRepository(DbContext);
        _cancellationToken = new CancellationToken();
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

    

}
