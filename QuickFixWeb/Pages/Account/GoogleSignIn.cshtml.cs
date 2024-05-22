using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Infrastructure.Dto;
using Microsoft.AspNetCore.Identity;
using Infrastructure;
using QuickFixWeb.Services.IService;
using HttpService.Services.IService;

namespace QuickFixWeb.Pages.Account
{
    public class GoogleSignInModel(IJwtTokenGenerator jwtTokenGenerator, ITokenProvider tokenProvider, IAuthService authService) : PageModel
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IAuthService _authService = authService;

        public async Task<IActionResult> OnGetAsync()
        {
            // Authenticate user using the external cookie
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
            {
                // Redirect to login page if authentication fails
                return RedirectToPage("/Account/Login");
            }

            

            // Get user's claims from the external authentication
            var userClaims = new List<Claim>(authResult.Principal.Claims);


            //Create token
            ApplicationUser user = new() { Email = userClaims.FirstOrDefault(c=>c.Type.Contains("email"))?.Value,
                                           Name = userClaims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))?.Value,
                                           Id = userClaims.FirstOrDefault(c => c.Type.Contains("nameidentifier")) ?.Value
                                         };
            //check user's email alrady exists in the DB
            var response =await _authService.CheckEmail(user.Email);
            if ((bool)response.Result)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["Message"] = "A user with the same Gmail address exists!";
                return RedirectToPage("/Account/Login");
            }

            response = await _authService.CheckName(user.Name);
            if ((bool)response.Result)
            {
                TempData["Message"] = "A user with the same name/author address exists!";
                return Page();
            }

            var token = _jwtTokenGenerator.GenerateToken(user, ["Customer"]);
            _tokenProvider.SetToken(token);

            var (principal, authenticationProperties) = _tokenProvider.PrepareSignInFromToken(token, true);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          //new ClaimsPrincipal(new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme)),
                                          principal,
                                          authenticationProperties
                                         );

            // Redirect to home page or any other protected page
            return RedirectToPage("/Index");
        }
    }
}
