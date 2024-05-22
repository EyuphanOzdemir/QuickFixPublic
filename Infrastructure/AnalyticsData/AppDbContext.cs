using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Infrastructure.AnalyticsData
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed your data here
            modelBuilder.Entity<Category>().HasData(
                new { Id = 1, Name = "MVC" },
                new { Id = 2, Name = "Razor Pages" }
            );

            modelBuilder.Entity<Tag>().HasData(
                new { Id = 1, Name = "ASP Net Core" },
                new { Id = 2, Name = "Tag 2" }
            );

            modelBuilder.Entity<Author>().HasData(
                new { Id = 1, Name = "Eyuphan" },
                new { Id = 2, Name = "Songul" }
            );
        }
    }
}
