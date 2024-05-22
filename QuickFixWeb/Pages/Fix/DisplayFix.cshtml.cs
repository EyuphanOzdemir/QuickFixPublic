using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Services.IService;
using static QuickFixWeb.Consts.Consts;

namespace QuickFixWeb.Pages.Fix
{
    public class DisplayFixModel : PageModel
    {
        private readonly IFixService _fixService;
        [BindProperty]
        public FixDto FixDto { get; set; }

        public DisplayFixModel(IFixService fixService)
        {
            _fixService = fixService;
        }

        public async Task OnGetAsync(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                // Handle missing ID here
                return;
            }

            var response = await _fixService.Find(Id);
            if (response != null && response.IsSuccess)
            {
                FixDto = JsonConvert.DeserializeObject<FixDto>(Convert.ToString(response.Result));
            }
            else
            {
                // Handle error response from service
                // For example, display an error message
                ViewData["Message"] = $"{Error_Indicator} Something is off:{response.Message}";
            }
        }
    }
}


