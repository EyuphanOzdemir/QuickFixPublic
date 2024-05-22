using Infrastructure;

namespace Infrastructure.AnalyticsData.Service
{
    public interface IAnalyticsDBService<T> where T:class
    {
        IEnumerable<T> Get(string filter);
        Task<bool> Add(string name);
        Task Merge(IEnumerable<T> source);
    }
}
