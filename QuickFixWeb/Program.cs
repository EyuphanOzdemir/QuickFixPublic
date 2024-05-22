using HttpService.Service;
using HttpService.Services;
using HttpService.Services.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Infrastructure;
using Infrastructure.Utility;
using QuickFixWeb.Services;
using QuickFixWeb.Services.IService;
namespace QuickFixWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var builder = WebApplication.CreateBuilder(args);
            GlobalValues.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
            GlobalValues.FixAPIBase = builder.Configuration["ServiceUrls:FixAPI"];
            GlobalValues.AnalyticsAPIBase = builder.Configuration["ServiceUrls:AnalyticsAPI"];
            GlobalValues.StripeAPIBase = builder.Configuration["ServiceUrls:StripeAPI"];
            GlobalValues.GoogleClientId = builder.Configuration["GoogleClientId"];
            GlobalValues.GoogleClientSecret = builder.Configuration["GoogleClientSecret"];
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IBaseHttpService, BaseHttpService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFixService, FixService>();
            builder.Services.AddScoped(typeof(AnalyticsHttpService<Category>));
            builder.Services.AddScoped(typeof(AnalyticsHttpService<Tag>));
            builder.Services.AddScoped(typeof(AnalyticsHttpService<Author>));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Set Google as the default challenge scheme
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            })
            .AddGoogle(options =>
            {
                options.ClientId = GlobalValues.GoogleClientId;
                options.ClientSecret = GlobalValues.GoogleClientSecret;
            });


            // Add services to the container.
            builder.Services.AddMvc();
			builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents();
			builder.Services.AddServerSideBlazor();
            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

            app.UseHttpsRedirection();
            // Configure to use the custom "Page Not Found" page for handling 404 errors
            app.UseStatusCodePagesWithReExecute("/NotFound");
            app.UseStaticFiles();

			app.UseRouting();

            app.UseAntiforgery();
            app.UseAuthorization();

			app.MapRazorPages();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("Referrer-Policy", "no-referrer-when-downgrade");
                await next();
            });

            app.Run();
		}
	}
}
