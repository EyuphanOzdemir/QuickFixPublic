using HttpService.Services.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Infrastructure.Utility;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace HttpService.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(GlobalValues.TokenCookie);
        }

        public string GetToken()
        {
            string token = null;
            bool hasToken = _contextAccessor.HttpContext.Request.Cookies.TryGetValue(GlobalValues.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
            // Inside your method where you generate and append the cookie
            var cookieOptions = new CookieOptions
            {
                // Specify the expiration time of the cookie
                Expires = DateTime.UtcNow.AddDays(30) // Set the expiration time (e.g., 1 hour from now)
            };
            _contextAccessor.HttpContext?.Response.Cookies.Append(GlobalValues.TokenCookie, token, cookieOptions);
        }

        public (ClaimsPrincipal, AuthenticationProperties) PrepareSignInFromToken(string token, bool rememberMe = true)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var email = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
            if (email != null)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, email));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)?.Value));
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
                var role = jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value ?? "Customer";
                identity.AddClaim(new Claim(ClaimTypes.Role, role));

                var principal = new ClaimsPrincipal(identity);

                // Set cookie expiration based on Remember Me option
                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddMonths(1) : DateTimeOffset.UtcNow.AddMinutes(30) // Adjust expiration time as needed
                };

                return (principal, authenticationProperties);
            }
            return (null, null);
        }
    }
}
