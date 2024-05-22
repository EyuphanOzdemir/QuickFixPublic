using System.Net;
using System.Text;

namespace AnalyticsAPI.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _username;
        private readonly string _password;

        public BasicAuthMiddleware(RequestDelegate next, string username, string password)
        {
            _next = next;
            _username = username;
            _password = password;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                // Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int separatorIndex = usernamePassword.IndexOf(':');
                var username = usernamePassword.Substring(0, separatorIndex);
                var password = usernamePassword.Substring(separatorIndex + 1);

                // Check credentials
                if (username == _username && password == _password)
                {
                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(ex.Message);
                    }
                    return;
                }
            }

            // Unauthorized response
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
