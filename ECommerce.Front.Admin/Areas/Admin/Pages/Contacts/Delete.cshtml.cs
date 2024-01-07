using ECommerce.Services.IServices;

namespace ECommerce.Front.Admin.Areas.Admin.Pages.Contacts;

public class DeleteModel(IContactService contactService) : PageModel
{
    [BindProperty] public Contact Contact { get; set; }
    [TempData] public string Message { get; set; }
    [TempData] public string Code { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var result = await contactService.GetById(id);
        if (result.Code == 0)
        {
            Contact = result.ReturnData;
            return Page();
        }

        return RedirectToPage("/Contacts/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
    }

    public async Task<IActionResult> OnPost(int id)
    {
        var result = await contactService.Delete(id);
        return RedirectToPage("/Contacts/Index",
            new { area = "Admin", message = result.Message, code = result.Code.ToString() });
        return Page();
    }
}