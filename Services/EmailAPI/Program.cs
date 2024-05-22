using QuickFix.Services.EmailAPI.Data;
using QuickFix.Services.EmailAPI.Extension;
using QuickFix.Services.EmailAPI.Messaging;
using QuickFix.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MessageBus.Interface;
using Microsoft.Extensions.Configuration;
using Infrastructure.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    //option.UseInMemoryDatabase("InMemoryDb");

    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

    /*TakeBack
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
                        dbContextOptionBuilder => dbContextOptionBuilder.MigrationsHistoryTable($"__EFMigrationsHistory_{Process.GetCurrentProcess().ProcessName}"));
    */
}, ServiceLifetime.Singleton);
EmailConfig emailConfig = new();
builder.Configuration.GetSection(nameof(EmailConfig)).Bind(emailConfig);
builder.Services.AddSingleton<IEmailSender>(new EmailSender(emailConfig));
builder.Services.AddSingleton<IEmailService, EmailService> ();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumerEmail>();
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.UseAzureServiceBusConsumer();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}