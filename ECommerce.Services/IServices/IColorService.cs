namespace ECommerce.Services.IServices;

public interface IColorService : IEntityService<ColorReadDto, ColorCreateDto, ColorUpdateDto>
{
    Task<ServiceResult<List<ColorReadDto>>> Filtering(string filter);
    Task<ServiceResult<List<ColorReadDto>>> Load();
    Task<ServiceResult<List<ColorReadDto>>> GetAll(string search = "", int pageNumber = 0, int pageSize = 10);
    Task<ServiceResult<ColorReadDto>> Add(ColorCreateDto color);
    Task<ServiceResult> Edit(ColorUpdateDto color);
    Task<ServiceResult> Delete(int id);
    Task<ServiceResult<ColorReadDto>> GetById(int id);
}