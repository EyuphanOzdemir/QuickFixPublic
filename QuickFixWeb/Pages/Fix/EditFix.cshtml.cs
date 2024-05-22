using static QuickFixWeb.Consts.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Services.IService;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using QuickFixWeb.Pages.Base;
using Microsoft.AspNetCore.Authorization;

namespace QuickFixWeb.Pages.Fix
{
    [Authorize]
    public class EditFixModel : BasePageModel
    {
        private readonly IFixService _fixService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        [BindProperty]
        public FixDto FixDto { get; set; }
        public string UserName { get; set; }
        public EditFixModel(IFixService fixService, IHttpContextAccessor httpContextAccessor)
        {
            _fixService = fixService;
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                // Handle missing ID here
                return NotFound();
            }

            var response = await _fixService.Find(id);
            if (response != null && response.IsSuccess)
            {
                FixDto = JsonConvert.DeserializeObject<FixDto>(Convert.ToString(response.Result));
                if (!FixDto.Author.Equals(UserName)) 
                {
                    //return RedirectToPage("/Account/AccessDenied");
                    return Forbid();
                }
                FixDto.Tags = FixDto.Tags ?? [];
                FixDto.Author = UserName;
                return Page();
            }
            else
            {
                // Handle error response from service
                // For example, display an error message
                return NotFound();
            }
        }

        public override async Task<IActionResult> OnPostAsync(object model)
        {
            var result = await base.OnPostAsync(); // Call the base method 
            if (result is not null)
                return result;

            // Deserialize the JSON string from listContentInput directly into a string array
            var listContentJson = Request.Form["ListContent"];
            string[] tags = JsonConvert.DeserializeObject<string[]>(listContentJson)??[];

            // Now, assign the deserialized tags to FixDto.Tags
            if (tags.Length != 0)
                FixDto.Tags = tags.Select(t=>t.Trim()).ToArray();

            var response = await _fixService.Update(FixDto);
            if (response != null && response.IsSuccess)
            {
                ViewData["Message"] = "Updated successfully";
            }
            else
            {
                // Handle error response from service
                // For example, display an error message
                ViewData["Message"] = $"{Error_Indicator} Something is off:{response.Message}";
            }
            FixDto.Tags = FixDto.Tags ?? [];
            return Page();
        }
    }
}
