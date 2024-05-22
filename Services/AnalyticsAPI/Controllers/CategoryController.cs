using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Infrastructure.Dto;
using Infrastructure.AnalyticsData.Service;
using AnalyticsAPI.Controllers;

namespace HttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController<Category>
    {
        public CategoryController(ILogger<Category> logger, IAnalyticsDBService<Category> dbService) : base(logger, dbService)
        {
        }
    }
}
