using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
<<<<<<< HEAD
using ECommerce.Infrastructure.DataContext;
using Microsoft.Data.Sqlite;
=======
using ECommerce.Infrastructure.DataContext.DataContext;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

namespace ECommerce.Repository.UnitTests.Base;

public class DbContextFake : DbContext
{
    private readonly SunflowerECommerceDbContext _databaseContext;
    private readonly HolooDbContext _holooDatabaseContext;

    public DbContextFake()
    {
<<<<<<< HEAD
        var connection = new SqliteConnection("datasource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
            .UseSqlite(connection)
            .Options;
        var optionsHoloo = new DbContextOptionsBuilder<HolooDbContext>()
            .UseSqlite(connection)
=======
        var options = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
            .UseInMemoryDatabase("SunFlower")
            .Options;
        var optionsHoloo = new DbContextOptionsBuilder<HolooDbContext>()
            .UseInMemoryDatabase("Holoo")
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
            .Options;
        _databaseContext = new SunflowerECommerceDbContext(
            options,
            new EphemeralDataProtectionProvider(),
            new ConfigurationManager());
        _holooDatabaseContext = new HolooDbContext(
            optionsHoloo);
    }

    public SunflowerECommerceDbContext CreateDatabaseContext()
    {
        _databaseContext.Database.EnsureCreated();
        return _databaseContext;
    }

    public void DeleteDatabaseContact()
    {
        _databaseContext.Database.EnsureDeleted();
    }

    public HolooDbContext CreateHolooDatabaseContext()
    {
        _holooDatabaseContext.Database.EnsureCreated();
        return _holooDatabaseContext;
    }

    public void DeleteHolooDatabaseContact()
    {
        _holooDatabaseContext.Database.EnsureDeleted();
    }
}