using HttpService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.AnalyticsData.Service;
using Infrastructure.Dto;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace AnalyticsAPI.Controllers
{
    public class BaseController<T> : Controller where T:BaseNamedEntity, new()
    {
        private readonly ILogger<T> _logger;
        private readonly IAnalyticsDBService<T> _dbService;

        public BaseController(ILogger<T> logger, IAnalyticsDBService<T> dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public ResponseDto Get([FromQuery]string filter = "")
        {
            var categories = _dbService.Get(filter);
            return new ResponseDto() { IsSuccess = true, Message = "", Result = categories.ToList() };
        }

        [HttpPost]
        public async Task<ResponseDto> Add(T entity)
        {
            await _dbService.Add(entity.Name);
            return new ResponseDto() { IsSuccess = true, Message = "", Result = true };
        }

        [HttpPost("Merge")]
        public async Task<ResponseDto> Merge(List<string> sourceStrings)
        {
            //sourceEntities.Clear
            var sourceEntities = sourceStrings.Select(s=>new T() {Name = s }).ToList();
            await _dbService.Merge(sourceEntities);
            return new ResponseDto() { IsSuccess = true, Message = "", Result = true };
        }
    }
}
