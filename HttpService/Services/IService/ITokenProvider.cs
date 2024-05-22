using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace HttpService.Services.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string GetToken();
        void ClearToken();

        (ClaimsPrincipal, AuthenticationProperties) PrepareSignInFromToken(string token, bool rememberMe = true);
    }
}
