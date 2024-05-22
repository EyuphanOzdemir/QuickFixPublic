using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Models;

namespace Infrastructure.AnalyticsData.Service
{
    public class BaseAnalyticsDBService<T>(AppDbContext db) : IAnalyticsDBService<T> where T : BaseNamedEntity, new()
    {
        private readonly AppDbContext _db = db;
        private DbSet<T> DbSet => _db.Set<T>();
        public async Task<bool> Add(string name)
        {
            if (name == null)
                return false;

            if (DbSet.SingleOrDefault(c => c.Name == name) != null)
                return true;

            await DbSet.AddAsync(new T() { Name = name });
            await _db.SaveChangesAsync();
            return true;
        }

        public IEnumerable<T> Get(string filter="")
        {
            // Return all items if filter is empty
            if (string.IsNullOrEmpty(filter))
            {
                return DbSet.ToList();
            }

            // Filter items based on the filter argument
            return [.. DbSet.Where(x=>x.Name.ToLower().Contains(filter.ToLower()??""))];
        }

        public async Task Merge(IEnumerable<T> source)
        {
            // Fetch existing categories from the database
            var setFromDatabase = Get();

            // Add new entities to the database
            var newEntities = source.Except(setFromDatabase).ToList();
            DbSet.AddRange(newEntities);

            // Delete categories from the database that are not present in the list
            var entitiesToDelete = setFromDatabase.Except(source).ToList();
            DbSet.RemoveRange(entitiesToDelete);

            // Save changes to the database
            await _db.SaveChangesAsync();
        }
    }
}
