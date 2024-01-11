using AutoMapper;
using ECommerce.Application.DataTransferObjects.Color;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.DataTransferObjectMappers;

namespace ECommerce.Application.DataTransferObjectMappers;

public class ColorDtoMapper : EntityDtoMapper<Color, ColorReadDto>, IColorDtoMapper
{
    public ColorUpdateDto MapColorToToUpdateDto(Color color, ColorUpdateDto colorUpdateDto)
    {
        Configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Color, ColorUpdateDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.ColorCode))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
            .ForMember(dest => dest.EditorUserId, opt => opt.MapFrom(src => src.EditorUserId));
        });
        Mapper = Configuration.CreateMapper();
        return Mapper.Map(color, colorUpdateDto);
    }
}
