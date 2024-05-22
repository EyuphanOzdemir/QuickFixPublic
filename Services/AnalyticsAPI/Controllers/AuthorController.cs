using AnalyticsAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infrastructure;
using Infrastructure.AnalyticsData.Service;
using Infrastructure.Dto;

namespace HttpService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : BaseController<Author>
    {
        public AuthorController(ILogger<Author> logger, IAnalyticsDBService<Author> dbService) : base(logger, dbService)
        {
        }
    }
}
