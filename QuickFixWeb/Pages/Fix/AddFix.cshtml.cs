using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Models.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Pages.Base;
using QuickFixWeb.Services.IService;
using System.Security.Claims;
using static QuickFixWeb.Consts.Consts;

namespace QuickFixWeb.Pages.Fix
{

    [Authorize]
    public class AddFixModel : BasePageModel
    {
        private readonly IFixService _fixService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public FixDto FixDto { get; set; }
        private string UserName { get; set; }
        private string Email { get; set; }

        public AddFixModel(IFixService fixService, IHttpContextAccessor httpContextAccessor)
        {
            _fixService = fixService;
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
            Email = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(claim => claim.Type.Contains("email")).Value;
        }

        public IActionResult OnGet()
        {
            FixDto = new() { Tags = [], Author = UserName };
            return Page();
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
            if (tags.Any())
                FixDto.Tags = tags.Select(t => t.Trim()).ToArray();

            FixDto.Author = UserName;
            FixDto.Email = Email;

            var response = await _fixService.Add(FixDto);
            if (response != null && response.IsSuccess)
            {
                //return RedirectToPage("/Fix/FixList");
                FixDto = new FixDto() { Tags = [], Author = UserName };
                ModelState.Clear();
                ViewData["Message"] = "Saved successfully";
                return Page();
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