using AutoMapper;
using ECommerce.Infrastructure.DataTransferObjectMappers;

namespace ECommerce.Application.DataTransferObjectMappers;

public class EntityDtoMapper<TSource, TDest> : IEntityDtoMapper<TSource, TDest>
{
    public static MapperConfiguration Configuration { get; set; }
    public static IMapper Mapper { get; set; }
    public EntityDtoMapper()
    {
        Configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TSource, TDest>();
        });
        Mapper = Configuration.CreateMapper();
    }
    public TDest CreateMapper(TSource source, TDest dest)
    {
        return Mapper.Map<TSource, TDest>(source, dest);
    }

    public IEnumerable<TDest> CreateMapper(IEnumerable<TSource> source, IEnumerable<TDest> dest)
    {
        return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDest>>(source, dest);
    }
}
