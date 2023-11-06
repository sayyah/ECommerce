<<<<<<< HEAD
﻿using AutoFixture;
using ECommerce.Infrastructure.DataContext;
=======
﻿using ECommerce.Infrastructure.DataContext.DataContext;
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)

namespace ECommerce.Repository.UnitTests.Base;

public abstract class BaseTests : IDisposable
{
    public readonly CancellationToken CancellationToken;
    public readonly DbContextFake Db;
    public readonly SunflowerECommerceDbContext DbContext;
    public readonly HolooDbContext HolooDbContext;
<<<<<<< HEAD
    protected readonly Fixture Fixture;

    protected BaseTests()
    {
        Fixture = new Fixture();
=======

    protected BaseTests()
    {
>>>>>>> 94b9a029 (Fixed #565 ddd layers and dot net 8)
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