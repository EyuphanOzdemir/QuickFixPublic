using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models.Dto;
using QuickFixWeb.Services.IService;
using System.Threading.Tasks;
using static QuickFixWeb.Consts.Consts;

namespace QuickFixWeb.Pages.Fix
{
    [Authorize]
    public class DeleteFixModel : PageModel
    {
        private readonly IFixService _fixService;


        public DeleteFixModel(IFixService fixService)
        {
            _fixService = fixService;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Handle missing ID here
                return NotFound();
            }

            var response = await _fixService.Delete(id);
            if (response != null && response.IsSuccess)
            {
                TempData["Message"] = "Deleted successfully";
                return RedirectToPage("/Fix/FixList");
            }
            else
            {
                // Handle error response from service
                // For example, display an error message

                ViewData["Message"] = $"{Error_Indicator}Something is off:{response.Message}";

                return Page();
            }
        }
    }
}
