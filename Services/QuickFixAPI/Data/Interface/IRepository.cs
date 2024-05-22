using Infrastructure;
using QuickFixAPI.Models;
using System.Linq.Expressions;

namespace QuickFixAPI.Data.Interface
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void InsertOne(TEntity entity);
        void InsertMany(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> ListAll();

        TEntity FindOne(Expression<Func<TEntity, bool>> filter);
        TEntity FindById(string id);
        void UpdateOne(Expression<Func<TEntity, bool>> filter, TEntity target);
        void DeleteOne(Expression<Func<TEntity, bool>> filter);

        void DeleteMany(Expression<Func<TEntity, bool>> filter);
        void DeleteById(string id);

        public IEnumerable<string> ListDistinctEntities<T>();

        public (IEnumerable<Fix> fixes, long count) Search(SearchFixParams searchFixParams);
    }
}