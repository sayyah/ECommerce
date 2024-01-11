using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Infrastructure.Base.Ioc;
public class ValidatedQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;
    private readonly IQueryValidator<TQuery> _queryValidator;

    public ValidatedQueryHandler(
        IQueryHandler<TQuery, TResult> queryHandler,
        IQueryValidator<TQuery> queryValidator)
    {
        _queryHandler = queryHandler;
        _queryValidator = queryValidator;
    }

    public async Task<TResult> HandleAsync(TQuery query)
    {
        await _queryValidator.ValidateAsync(query);
        TResult result = await _queryHandler.HandleAsync(query);
        return result;
    }
}