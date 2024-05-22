using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuickFixWeb.Pages
{
    public class NotFoundModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
