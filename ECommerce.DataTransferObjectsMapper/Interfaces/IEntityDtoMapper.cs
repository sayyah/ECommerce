
namespace ECommerce.DataTransferObjectsMapper.Interfaces;

public interface IEntityDtoMapper<TSource, TDest>
{
    TDest CreateMapper(TSource source, TDest dest);
    IEnumerable<TDest> CreateMapper(IEnumerable<TSource> source, IEnumerable<TDest> dest);
}
