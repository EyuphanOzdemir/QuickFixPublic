using Microsoft.EntityFrameworkCore;
using Infrastructure;
using MongoDB.Driver;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;
using System.Linq.Expressions;
using CoreModels = Infrastructure;

namespace QuickFixAPI.Data
{
    public class TestRepositoryFix<TEntity> : IRepository<Fix> where TEntity : BaseEntity
	{
		private List<Fix> _entities;

		public TestRepositoryFix()
		{
			_entities = SeedData.Fixes();
		}

		public void InsertOne(Fix entity)
		{
			_entities.Add(entity);
		}

		public void InsertMany(IEnumerable<Fix> entities)
		{
			_entities.AddRange(entities);
		}

		public IEnumerable<Fix> Find(Expression<Func<Fix, bool>> filter)
		{
			return _entities.Where(filter.Compile());
		}

		public IEnumerable<Fix> ListAll()
		{
			return _entities;
		}

		public	Fix FindOne(Expression<Func<Fix, bool>> filter)
		{
			return _entities.FirstOrDefault(filter.Compile());
		}

		public Fix FindById(string id)
		{
			return _entities.FirstOrDefault(e => e.Id == id);
		}

		public void UpdateOne(Expression<Func<Fix, bool>> filter, Fix target)
		{
			var entityToUpdate = _entities.FirstOrDefault(filter.Compile());
			if (entityToUpdate != null)
			{
				var fix = target as Fix;
				entityToUpdate.Id = fix.Id;
				entityToUpdate.Category = fix.Category;
				entityToUpdate.Tags = fix.Tags;
				entityToUpdate.Author = fix.Author;
				entityToUpdate.Solution = fix.Solution;
				entityToUpdate.Title = fix.Title;
			}
		}

		public void DeleteOne(Expression<Func<Fix, bool>> filter)
		{
			var entityToDelete = _entities.FirstOrDefault(filter.Compile());
			if (entityToDelete != null)
			{
				_entities.Remove(entityToDelete);
			}
		}

        public void DeleteMany(Expression<Func<Fix, bool>> filter)
        {
			var x = filter.Compile();
			Predicate<Fix> predicate = f => x(f);
			_entities.RemoveAll(predicate);
        }

        public void DeleteById(string id)
		{
			var entityToDelete = _entities.FirstOrDefault(e => e.Id == id);
			if (entityToDelete != null)
			{
				_entities.Remove(entityToDelete);
			}
		}

        private IEnumerable<string> ListDistinctCategories()
        {
			return _entities.Select(f => f.Category).Distinct().ToList();
        }

        private IEnumerable<string> ListDistinctAuthors()
        {
            return _entities.Select(f => f.Author).Distinct().ToList();
        }

        private IEnumerable<string> ListDistinctTags()
        {
            return _entities.SelectMany(f => f.Tags).Distinct().ToList();
        }

        public IEnumerable<string> ListDistinctEntities<T>()
        {
            var typeOfT = typeof(T);
            return typeOfT switch
            {
                var t when t == typeof(CoreModels.Category) => ListDistinctCategories(),
                var t when t == typeof(CoreModels.Tag) => ListDistinctTags(),
                var t when t == typeof(CoreModels.Author) => ListDistinctAuthors(),
                _ => Enumerable.Empty<string>()
            };
        }

        public (IEnumerable<Fix> fixes, long count) Search(SearchFixParams searchFixParams)
        {
            var query = _entities.AsQueryable(); // Start with all entities

            if (!string.IsNullOrEmpty(searchFixParams.Category))
            {
                query = query.Where(e => e.Category.ToLower().Contains(searchFixParams.Category.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Author))
            {
                query = query.Where(e => e.Author.ToLower().Contains(searchFixParams.Author.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Tag))
            {
				query.Where(f => string.Join(" ",f.Tags).ToLower().Contains(searchFixParams.Tag.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Title))
            {
                query = query.Where(e => e.Title.ToLower().Contains(searchFixParams.Title.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Solution))
            {
                query = query.Where(e => e.Solution.ToLower().Contains(searchFixParams.Solution.ToLower()));
            }

            var fixes = query.ToList();
			var count = fixes.Count;
			return (fixes, count);
		}
    }

}
