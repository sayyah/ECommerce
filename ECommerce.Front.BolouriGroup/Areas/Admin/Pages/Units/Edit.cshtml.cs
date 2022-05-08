﻿using System.Threading.Tasks;
using Entities;
using Entities.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Services.IServices;

namespace ArshaHamrah.Areas.Admin.Pages.Units
{
    public class EditModel : PageModel
    {
        private readonly IUnitService _unitService;

        public EditModel(IUnitService unitService)
        {
            _unitService = unitService;
        }
        [BindProperty] public Unit Unit { get; set; }
        [TempData] public string Message { get; set; }
        [TempData] public string Code { get; set; }

        public async Task OnGet(int id)
        {
            var result = await _unitService.GetById(id);
            Unit = result.ReturnData;
        }

        public async Task<IActionResult> OnPost()
        {
            
          
            if (ModelState.IsValid)
            {
                var result = await _unitService.Edit(Unit);
                Message = result.Message;
                Code = result.Code.ToString();
                if (result.Code == 0)
                    return RedirectToPage("/Units/Index",
                        new {area = "Admin", message = result.Message, code = result.Code.ToString()});
                Message = result.Message;
                Code = result.Code.ToString();
                ModelState.AddModelError("", result.Message);
            }

            return Page();
        }
    }
}