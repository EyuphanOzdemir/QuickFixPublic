using HttpService.Services.IService;
using Infrastructure;
using Infrastructure.AnalyticsData.Service;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;

namespace QuickFixAPI.Services;
public class BackgroundServiceDB : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRepository<Fix> _mongoDbFix;
    private readonly MergeService<Category> _categoryMergeService;
    private readonly MergeService<Tag> _tagMergeService;
    private readonly MergeService<Author> _authorMergeService;


    public BackgroundServiceDB(IServiceScopeFactory serviceScopeFactory, 
                               IRepository<Fix> db,
                               MergeService<Category> categoryMergeService,
                               MergeService<Tag> tagMergeService,
                               MergeService<Author> authorMergeService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mongoDbFix = db;
        _categoryMergeService = categoryMergeService;
        _tagMergeService = tagMergeService;
        _authorMergeService = authorMergeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000);
            // Perform background tasks here
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                //merge categories, tags and authors into analytics db
                await _categoryMergeService.Merge();
                await _tagMergeService.Merge();
                await _authorMergeService.Merge();
            }
            // Sleep for some time before the next iteration
            await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken); // Example: Delay for 15 minutes
        }
    }
}