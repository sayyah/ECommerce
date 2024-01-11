namespace ECommerce.Application.Base.Services.Interfaces
{
    public interface ICommnadValidator<TCommand>
        where TCommand : ICommand
    {
        Task ValidateAsync(TCommand command);
    }
}
