using ECommerce.Infrastructure.DataContext;
using ECommerce.Infrastructure.Repository;
using AutoFixture;


namespace ECommerce.Handler.UnitTests.Base
{
    public abstract class BaseTests : IDisposable
    {
        public readonly CancellationToken CancellationToken;
        public readonly DbContextFake Db;
        public readonly SunflowerECommerceDbContext DbContext;
        public readonly HolooDbContext HolooDbContext;
        protected readonly Fixture Fixture;
        protected readonly UnitOfWork UnitOfWork;

        protected BaseTests()
        {
            Db = new DbContextFake();
            DbContext = Db.CreateDatabaseContext();
            HolooDbContext = Db.CreateHolooDatabaseContext();
            UnitOfWork = new UnitOfWork(DbContext, HolooDbContext);
            Fixture = new Fixture();
            CancellationToken = new CancellationToken();
        }

        public void Dispose()
        {
            Db.DeleteDatabaseContact();
            Db.DeleteHolooDatabaseContact();
        }
    }
}
