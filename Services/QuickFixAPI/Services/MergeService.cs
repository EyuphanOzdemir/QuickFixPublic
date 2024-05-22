using HttpService.Services;
using Infrastructure;
using Infrastructure.AnalyticsData.Service;
using Infrastructure.Models;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;
using CoreModels = Infrastructure;

namespace QuickFixAPI.Services
{
    public class MergeService<T>(IRepository<Fix> mongoDBService,
                                 AnalyticsHttpService<T> analyticsHttpService
                                 ) where T:BaseNamedEntity, new()
    {
        private readonly IRepository<Fix> _mongoDbFix = mongoDBService;
        private readonly AnalyticsHttpService<T> _analyticsHttpService = analyticsHttpService;
        public async Task Merge() 
        {
          var distinctEntities = _mongoDbFix.ListDistinctEntities<T>().ToList();
          await _analyticsHttpService.Merge(distinctEntities);
        }
    }
}
