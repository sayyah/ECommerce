namespace ECommerce.Application.Base.Services.Interfaces;
public interface IQueryValidator<TQuery>
    where TQuery : IQuery
{
    Task ValidateAsync(TQuery query);
}