namespace ECommerce.API.Utilities;
public interface IDtoMapper
{
    TDestination Map<TDestination>(object source);
    TDestination Map<TSource, TDestination>(TSource source);
}