using ECommerce.API.DataTransferring.Colors;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.DataTransferObjectMappers;

public interface IColorDtoMapper : IEntityDtoMapper<Color, ColorReadDto>
{
    IEnumerable<ColorReadDto> CreateMapper(IEnumerable<Color> colors, IEnumerable<ColorReadDto> colorsRead);
    ColorUpdateDto MapColorToToUpdateDto(Color color, ColorUpdateDto colorUpdateDto);
}
