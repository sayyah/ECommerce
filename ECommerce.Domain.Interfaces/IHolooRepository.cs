namespace ECommerce.Domain.Interfaces;

public interface IHolooRepository<T> : IDisposable
{
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
}
