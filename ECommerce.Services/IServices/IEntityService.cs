namespace ECommerce.Services.IServices;

public interface IEntityService<TRead, in TCreate, in TUpdate> :
    IReadEntityService<TRead>,
    ICreateEntityService<TCreate>,
    IUpdateEntityService<TUpdate>,
    IDeleteEntityService,
    IReturnEntityService
{
}