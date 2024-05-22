using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class UnauthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _redirectPage;

    public UnauthorizeAttribute(string redirectPage)
    {
        _redirectPage = redirectPage;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check if the user is authenticated
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            // If authenticated, redirect to the specified page
            context.Result = new RedirectToPageResult(_redirectPage);
        }
        else { return;  }
    }
}
