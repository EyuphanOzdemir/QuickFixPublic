using CoreModels = Infrastructure;
using MongoDB.Driver;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;
using System.Linq.Expressions;
using Infrastructure;
using MongoDB.Bson;
using Amazon.SecurityToken.Model;
namespace QuickFixAPI.Data
{
    public class MongoRepositoryFix<TEntity> : IRepository<Fix> where TEntity : BaseEntity
    {
		private readonly IMongoCollection<Fix> _collection;

		public MongoRepositoryFix(IMongoDatabase database)
		{
			_collection = database.GetCollection<Fix>(nameof(Fix).ToLower());
		}

		public void InsertOne(Fix entity)
		{
			entity.CreateDate = DateTime.Now;
            _collection.InsertOne(entity);
		}

		public void InsertMany(IEnumerable<Fix> entities)
		{
			_collection.InsertMany(entities);
		}

		public IEnumerable<Fix> Find(Expression<Func<Fix, bool>> filter)
		{
			return _collection.Find(filter).ToEnumerable();
		}

		public IEnumerable<Fix> ListAll()
		{
			return _collection.Find(_ => true).ToEnumerable();
		}

		public Fix FindOne(Expression<Func<Fix, bool>> filter)
		{
			return _collection.Find(filter).FirstOrDefault();
		}

		public Fix FindById(string id)
		{
			return _collection.Find(fix=>fix.Id.Equals(id)).FirstOrDefault();
		}

		public void UpdateOne(Expression<Func<Fix, bool>> filter, Fix target)
		{
    		var update = Builders<Fix>.Update
				.Set(f => f.Title, target.Title)
				.Set(f => f.Author, target.Author)
				.Set(f => f.Solution, target.Solution)
				.Set(f => f.Category, target.Category)
				.Set(f => f.Tags, target.Tags);
				_collection.UpdateOne(filter, update as UpdateDefinition<Fix>);
		}

		public void DeleteOne(Expression<Func<Fix, bool>> filter)
		{
			_collection.DeleteOne(filter);
		}

        public void DeleteMany(Expression<Func<Fix, bool>> filter)
        {
            _collection.DeleteMany(filter);
        }

        public void DeleteById(string id)
		{
			_collection.DeleteOne(fix=>fix.Id.Equals(id));
		}

        public IEnumerable<string> ListDistinctEntities<T>()
		{
            var typeOfT = typeof(T);
            return typeOfT switch
            {
                var t when t == typeof(CoreModels.Category) => ListDistinctCategories(),
                var t when t == typeof(CoreModels.Tag) => ListDistinctTags(),
                var t when t == typeof(CoreModels.Author) => ListDistinctAuthors(),
                _ => []
            };
        }

        private IEnumerable<string> ListDistinctCategories()
        {
            var distinctCategories = _collection.Distinct(x => x.Category, FilterDefinition<Fix>.Empty);

            return distinctCategories.ToEnumerable();
        }

        private IEnumerable<string> ListDistinctTags()
        {
            var distinctTags = _collection.AsQueryable()
                .SelectMany(f => f.Tags)
                .Distinct();

            return distinctTags;
        }

        private IEnumerable<string> ListDistinctAuthors()
        {
            var distinctAuthors = _collection.Distinct(x => x.Author, FilterDefinition<Fix>.Empty);

            return distinctAuthors.ToEnumerable();
        }

        public (IEnumerable<Fix> fixes, long count) Search(SearchFixParams searchFixParams)
        {
            var filter = Builders<Fix>.Filter.Empty;

            if (!string.IsNullOrEmpty(searchFixParams.Category))
            {
                filter &= Builders<Fix>.Filter.Regex(x => x.Category, new BsonRegularExpression(searchFixParams.Category, "i"));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Author))
            {
                filter &= Builders<Fix>.Filter.Regex(x => x.Author, new BsonRegularExpression(searchFixParams.Author, "i"));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Tag))
            {
                filter &= Builders<Fix>.Filter.Eq("Tags", searchFixParams.Tag);
            }

            if (!string.IsNullOrEmpty(searchFixParams.Title))
            {
                filter &= Builders<Fix>.Filter.Regex(x => x.Title, new BsonRegularExpression(searchFixParams.Title, "i"));
            }

            if (!string.IsNullOrEmpty(searchFixParams.Solution))
            {
                filter &= Builders<Fix>.Filter.Regex(x => x.Solution, new BsonRegularExpression(searchFixParams.Solution, "i"));
            }

            // Execute a count query to get the total count of fixes matching the search criteria
            long count = _collection.CountDocuments(filter);

            // Calculate skip count based on PageId
            List<Fix> fixes = [];
            if (searchFixParams.PageId > 0)
            {
                int pageSize = 10;
                int skipCount = (searchFixParams.PageId - 1) * pageSize;
                // Fetch paginated results with sorting by CreateDate in descending order
                fixes = _collection.Find(filter)
                                       .Sort(Builders<Fix>.Sort.Descending(x => x.CreateDate))
                                       .Skip(skipCount) // Skip records based on page number
                                       .Limit(pageSize)      // Limit records per page to 10
                                       .ToList();
            }
            else
                fixes = _collection.Find(filter)
                       .Sort(Builders<Fix>.Sort.Descending(x => x.CreateDate))
                       .ToList();

            return (fixes, count);
        }
    }
}