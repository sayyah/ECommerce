﻿using System.Threading.Tasks;
using Entities;
using Entities.Helper;
using Entities.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Services.IServices;

namespace ArshaHamrah.Areas.Admin.Pages.SlideShows
{
    public class CreateModel : PageModel
    {
        private readonly ISlideShowService _slideShowService;
        private readonly IHostEnvironment _environment;
        private readonly IImageService _imageService;

        public CreateModel(ISlideShowService slideShowService, IImageService imageService, IHostEnvironment environment)
        {
            _slideShowService = slideShowService;
            _imageService = imageService;
            _environment = environment;
        }

        [BindProperty] public SlideShowViewModel SlideShow { get; set; }

        [BindProperty] public IFormFile Upload { get; set; }

        [TempData] public string Message { get; set; }

        [TempData] public string Code { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (Upload == null)
            {
                Message = "لطفا عکس را انتخاب کنید";
                Code = ServiceCode.Error.ToString();
                return Page();
            }
            var fileName = (await _imageService.Upload(Upload, "Images/SlideShows", _environment.ContentRootPath))
                .ReturnData;
            SlideShow.ImagePath = $"/{fileName[0]}/{fileName[1]}/{fileName[2]}";

            if (ModelState.IsValid)
            {
                var result = await _slideShowService.Add(SlideShow);
                if (result.Code == 0)
                    return RedirectToPage("/SlideShows/Index",
                        new {area = "Admin", message = result.Message, code = result.Code.ToString()});
                Message = result.Message;
                Code = result.Code.ToString();
                ModelState.AddModelError("", result.Message);
            }

            return Page();
        }
    }
}