using ECommerce.Services.IServices;

namespace ECommerce.Front.BolouriGroup.Areas.Admin.Pages.Colors;

public class EditModel : PageModel
{
    private readonly IColorService _colorService;

    public EditModel(IColorService colorService)
    {
        _colorService = colorService;
    }

    [BindProperty] public ColorUpdateDto Color { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task OnGet(int id)
    {
        var colorReadDto = await _colorService.GetById(id);
        var config = new MapperConfiguration(cfg => cfg.CreateMap<ColorReadDto, ColorUpdateDto>());
        var _mapper = config.CreateMapper();
        var result = _mapper.Map(colorReadDto, Color);
        Color = result;
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var result = await _colorService.Edit(Color);
            Message = result.Message;
            Code = result.Code.ToString();
            if (result.Code == 0)
                return RedirectToPage("/Colors/Index",
                    new { area = "Admin", message = result.Message, code = result.Code.ToString() });
            Message = result.Message;
            Code = result.Code.ToString();
            ModelState.AddModelError("", result.Message);
        }

        return Page();
    }
}