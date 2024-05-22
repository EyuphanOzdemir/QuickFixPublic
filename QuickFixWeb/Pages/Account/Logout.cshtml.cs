// Logout.cshtml.cs

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace QuickFixWeb.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page or any other page after signout
            return RedirectToPage("/Index");
        }
    }
}
