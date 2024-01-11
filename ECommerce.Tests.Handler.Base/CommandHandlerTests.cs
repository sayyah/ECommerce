using ECommerce.Application.Base.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECommerce.Tests.Handler.Base;

public abstract class CommandHandlerTests<TCommandHandler, TCommand, TResult>
    where TCommandHandler : class, ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand<TResult> 
{
    protected TCommandHandler CommandHandler { get; private set; }
    protected TCommand Command { get; private set; }

    [TestInitialize]
    public virtual void TestInitialize()
    {
        InitializeDependenciesMocks();
        CommandHandler = InitializeCommandHandler();
        Command = CreateValidCommand();
        SetupDependenciesMocks();
    }

    protected abstract void InitializeDependenciesMocks();

    protected abstract TCommandHandler InitializeCommandHandler();

    protected abstract TCommand CreateValidCommand();

    protected virtual void SetupDependenciesMocks()
    {
    }

    [TestMethod]
    public async Task HandleAsync_CommandIsValid_NoExceptionIsThrown()
    {
        await CommandHandler.HandleAsync(Command, CancellationToken.None);
    }
}