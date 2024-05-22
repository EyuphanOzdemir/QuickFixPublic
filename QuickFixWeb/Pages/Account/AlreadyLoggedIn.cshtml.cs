using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuickFixWeb.Pages.Account
{
    [Authorize]
    public class AlreadyLoggedInModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "You are already logged in. Please log out first!";
        }
    }
}
