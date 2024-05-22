using QuickFix.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuickFix.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailLog> EmailLogs { get; set; }

        
    }
}
