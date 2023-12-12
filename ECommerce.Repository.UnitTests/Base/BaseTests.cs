using AutoFixture;
using ECommerce.Infrastructure.DataContext;

namespace ECommerce.Repository.UnitTests.Base;

public abstract class BaseTests : IDisposable
{
    public readonly CancellationToken CancellationToken;
    public readonly DbContextFake Db;
    public readonly SunflowerECommerceDbContext DbContext;
    public readonly HolooDbContext HolooDbContext;
    protected readonly Fixture Fixture;

    protected BaseTests()
    {
        Fixture = new Fixture();
        Db = new DbContextFake();
        CancellationToken = new CancellationToken();
        DbContext = Db.CreateDatabaseContext();
        HolooDbContext = Db.CreateHolooDatabaseContext();
    }

    public void Dispose()
    {
        Db.DeleteDatabaseContact();
        Db.DeleteHolooDatabaseContact();
    }
}