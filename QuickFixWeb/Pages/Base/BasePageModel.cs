// BasePage.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickFixWeb.Helpers;

namespace QuickFixWeb.Pages.Base
{
    public class BasePageModel : PageModel
    {
        public virtual Task<IActionResult> OnPostAsync(object model=null)
        {
            // Common logic for OnPost
            if (!ModelStateHelper.HandleModelStateErrors(ModelState, this))
            {
                // Return null if there are no model state errors
                return Task.FromResult<IActionResult>(null);
            }

            // Return Page() if there are model state errors
            return Task.FromResult<IActionResult>(Page());
        }
    }
}