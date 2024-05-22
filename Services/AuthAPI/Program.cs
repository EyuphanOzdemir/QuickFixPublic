using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using MessageBus.Interface;
using HttpService.Services.IService;
using HttpService.Service;
using AuthAPI.Data;
using AuthAPI.Service.IService;
using AuthAPI.Service;


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

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllers();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageBusPublisher, MessageBus.Implementation.MessageBusPublisher>();
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API");
        c.RoutePrefix = string.Empty;
    }
});
app.UseHttpsRedirection();
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
    }
}
