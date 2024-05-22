using AnalyticsAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Infrastructure.AnalyticsData.Service;
using Infrastructure.Dto;

namespace HttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : BaseController<Tag>
    {
        public TagController(ILogger<Tag> logger, IAnalyticsDBService<Tag> dbService) : base(logger, dbService)
        {
        }
    }
}
