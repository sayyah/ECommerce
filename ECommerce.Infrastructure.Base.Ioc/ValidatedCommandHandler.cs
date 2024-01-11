using ECommerce.Application.Base.Services.Interfaces;

namespace ECommerce.Infrastructure.Base.Ioc;
public class ValidatedCommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _commandHandler;
    private readonly ICommnadValidator<TCommand> _commnadValidator;

    public ValidatedCommandHandler(
        ICommandHandler<TCommand, TResult> commandHandler,
        ICommnadValidator<TCommand> commnadValidator)
    {
        _commandHandler = commandHandler;
        _commnadValidator = commnadValidator;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await _commnadValidator.ValidateAsync(command);
        TResult result = await _commandHandler.HandleAsync(command , cancellationToken);
        return result;
    }
}