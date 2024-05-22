using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace QuickFixWeb.Pages.Account
{
    public class ExternalLoginModel : PageModel
    {
        public IActionResult OnGet(string provider)
        {
            // Redirect to Google authentication endpoint
            return Challenge(new AuthenticationProperties { RedirectUri = "/Account/GoogleSignIn" }, provider);
        }
    }
}
