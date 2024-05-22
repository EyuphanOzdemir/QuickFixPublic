using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure.AnalyticsData;
using Serilog;
using Infrastructure.Utility;
using Infrastructure.Extensions;
using Infrastructure.AnalyticsData.Service;
using Infrastructure;
using AnalyticsAPI.Middlewares;
using Microsoft.Extensions.Configuration;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //options.UseInMemoryDatabase("InMemoryDb");

    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

    /*
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
			    		 dbContextOptionBuilder => dbContextOptionBuilder.MigrationsHistoryTable($"__EFMigrationsHistory_{Process.GetCurrentProcess().ProcessName}"));
    */

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
/*
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, Array.Empty<string>()
        }
    });
});
*/

builder.Services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog(SerilogConfiguration.GetSeriLogger());
});
builder.Services.AddControllers();
//builder.AddJWTAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAnalyticsDBService<Category>, BaseAnalyticsDBService<Category>>();
builder.Services.AddScoped<IAnalyticsDBService<Tag>, BaseAnalyticsDBService<Tag>>();
builder.Services.AddScoped<IAnalyticsDBService<Author>, BaseAnalyticsDBService<Author>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    if (!app.Environment.IsDevelopment())
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuickFix API");
        c.RoutePrefix = string.Empty;
    }
});
app.UseHttpsRedirection();
app.UseMiddleware<BasicAuthMiddleware>("robespierre", "Yarasa13_");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.IsRelational() && _db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
        else
            _db.Database.EnsureCreated();
    }
}
