using ECommerce.Infrastructure.DataContext;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Repository.UnitTests.Base;

public class DbContextFake : DbContext
{
    private readonly SunflowerECommerceDbContext _databaseContext;
    private readonly HolooDbContext _holooDatabaseContext;

    public DbContextFake()
    {
        var connection = new SqliteConnection("datasource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<SunflowerECommerceDbContext>()
            .UseSqlite(connection)
            .Options;
        var optionsHoloo = new DbContextOptionsBuilder<HolooDbContext>()
            .UseSqlite(connection)
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