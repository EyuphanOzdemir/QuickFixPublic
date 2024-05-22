using HttpService.Services.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Pages.Base;
using QuickFixWeb.Services.IService;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


using static QuickFixWeb.Consts.Consts;
using System.ComponentModel;

namespace QuickFixWeb.Pages.Account
{
    [Unauthorize("/Account/AlreadyLoggedIn")]
    public class LoginModel : BasePageModel
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;


        [BindProperty]
        [Required]
        public LoginRequestDto Credentials { get; set; }

        [BindProperty]
        [DisplayName("Remember me")]
        public bool RememberMe { get; set; }


        public LoginModel(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        public override async Task<IActionResult> OnPostAsync(object model)
        {
            var result = await base.OnPostAsync(); // Call the base method 
            if (result is not null)
                return result;

            try
            {
                // Authenticate user
                var responseDto = await _authService.LoginAsync(Credentials);

                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Authentication success
                    LoginResponseDto loginResponseDto =
                        JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                    await SignInUser(loginResponseDto);
                    _tokenProvider.SetToken(loginResponseDto.Token);
                    return RedirectToPage("/Index");
                }
                else
                {
                    // Authentication failed
                    ViewData["Message"] = Error_Indicator + responseDto?.Message ?? "Login failed. Please check your credentials.";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewData["Message"] = $"{Error_Indicator}Something went wrong:{ex.Message}";
                Log.Error(ex, "Error during login");
            }

            return Page();
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var (principal, authenticationProperties) = _tokenProvider.PrepareSignInFromToken(model.Token, RememberMe);
            await HttpContext.SignInAsync(principal, authenticationProperties);
        }
    }
}
