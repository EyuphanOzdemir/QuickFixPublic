using AutoMapper;
using QuickFixAPI.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using QuickFixAPI.Data;
using QuickFixAPI.Middlewares;
using Serilog;
using QuickFixAPI.Data.Interface;
using Infrastructure.Extensions;
using QuickFixAPI.Extensions;
using HttpService.Services.IService;
using Infrastructure.Utility;
using HttpService.Services;
using QuickFixAPI.Filters;
using QuickFixAPI.Services;
using QuickFixAPI.Models;
using Infrastructure.AnalyticsData.Service;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Infrastructure.AnalyticsData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");

    /*
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
			    		 dbContextOptionBuilder => dbContextOptionBuilder.MigrationsHistoryTable($"__EFMigrationsHistory_{Process.GetCurrentProcess().ProcessName}"));
    */

}, ServiceLifetime.Singleton);

// Add services to the container.
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
GlobalValues.AnalyticsAPIBase = builder.Configuration["ServiceUrls:AnalyticsAPI"];
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<IBaseHttpService, BaseHttpService>();
builder.Services.AddSingleton(typeof(AnalyticsHttpService<Category>));
builder.Services.AddSingleton(typeof(AnalyticsHttpService<Tag>));
builder.Services.AddSingleton(typeof(AnalyticsHttpService<Author>));
builder.Services.AddSingleton<MergeService<Category>>();
builder.Services.AddSingleton<MergeService<Tag>>();
builder.Services.AddSingleton<MergeService<Author>>();
builder.Services.AddLogging(builder =>
{
	builder.ClearProviders();
	builder.AddSerilog(SerilogConfiguration.GetSeriLogger());
});
builder.Services.AddMemoryCache();
// Add response caching services
builder.Services.AddResponseCaching();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
builder.AddJWTAuthentication();
builder.AddMongoDatabase(builder.Configuration.GetConnectionString("DefaultConnection"), "quickfix");
builder.Services.AddSingleton<IRepository<Fix>, MongoRepositoryFix<Fix>>();
//builder.Services.AddSingleton<IAnalyticsDBService<Category>, BaseAnalyticsDBService<Category>>();
//builder.Services.AddSingleton<IAnalyticsDBService<Tag>, BaseAnalyticsDBService<Tag>>();
//builder.Services.AddSingleton<IAnalyticsDBService<Author>, BaseAnalyticsDBService<Author>>();
builder.Services.AddScoped<CheckTestUserAttribute>();
builder.Services.AddAuthorization();
builder.Services.AddSingleton<MergeService<Category>>();
builder.Services.AddSingleton<MergeService<Tag>>();
builder.Services.AddSingleton<MergeService<Author>>();
builder.Services.AddHostedService<BackgroundServiceDB>();
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}
else
	app.UseMiddleware<GlobalExceptionMiddleware>();


// Enable response caching middleware
app.UseResponseCaching();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	if (!app.Environment.IsDevelopment())
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.jsonxxx", "QuickFix API");
		c.RoutePrefix = string.Empty;
	}
});

// Use CORS
app.UseCors();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
