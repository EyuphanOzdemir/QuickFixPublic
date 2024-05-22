using AutoMapper;
using HttpService.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using CoreModels = Infrastructure;
using Infrastructure.Dto;
using Infrastructure.Models.Dto;
using MongoDB.Driver;
using QuickFixAPI.Data.Interface;
using QuickFixAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using HttpService.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace QuickFixAPI.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class FixController(IRepository<Fix> fixRepository,
                              ILogger<FixController> logger,
                              IMemoryCache cache,
                              AnalyticsHttpService<CoreModels.Category> categoryService,
                              AnalyticsHttpService<CoreModels.Author> authorService,
                              AnalyticsHttpService<CoreModels.Tag> tagService,
                              IMapper mapper) : ControllerBase
	{
		private readonly IRepository<Fix> _fixRepository = fixRepository;
		private readonly ILogger<FixController> _logger = logger;
		private readonly IMemoryCache _cache = cache;
        private readonly AnalyticsHttpService<CoreModels.Category> _categoryService = categoryService;
        private readonly AnalyticsHttpService<CoreModels.Tag> _tagService = tagService;
        private readonly AnalyticsHttpService<CoreModels.Author> _authorService = authorService;
        private readonly IMapper _mapper = mapper;
        private ResponseDto _response = new();

        [HttpGet]
		public IActionResult ListAll() 
		{
			_logger.LogInformation("Listing all fixes.......");
			try
			{
				// Check if data exists in cache
				if (!_cache.TryGetValue<IEnumerable<Fix>>("AllFixes", out var fixes))
				{
					// If not found in cache, fetch data from repository and cache it
					fixes = _fixRepository.ListAll().ToList();
					_cache.Set("AllFixes", fixes, TimeSpan.FromSeconds(3)); // Cache for 10 seconds
				}
				List<FixDto> fixList = _mapper.Map<List<FixDto>>(fixes);
				_response.Result = fixList;

				return Ok(_response);
			}
			catch (Exception ex)
			{
                _response.IsSuccess = false;
                _response.Message = $"An error occurred while processing your request: {ex.Message}";
                _logger.LogError(ex, "Error occurred while listing all fixes.");
				return BadRequest(_response);
			}
		}

        [Authorize]
        [HttpGet]
		[Route("Find/{id}")]
		[ResponseCache(Duration = 600, VaryByQueryKeys = ["id"])] // Cache response for 600 seconds with varying by query parameter "id"
		public IActionResult Find(string id)
		{
			_logger.LogInformation("Fetching a fix...");
			try
			{
				var fix =_mapper.Map<FixDto>(_fixRepository.FindById(id));
				_response.Result = fix;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while finding a fix");
				_response.Message = $"An error occurred while processing your request. {ex.Message}";
                return BadRequest(_response);
			}
		}

        [HttpGet]
        [Route("Search")]
        public IActionResult Search([FromQuery]SearchFixParams searchFixParams)
        {
            _logger.LogInformation("Fetching a fix based on search...");

            try
            {
				var (fixes, count) = _fixRepository.Search(searchFixParams);
				var fixDtos = _mapper.Map<List<FixDto>>(fixes);
                _response.Result = new SearchFixResult(fixDtos, count);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching a fix");
                _response.Message = $"An error occurred while processing your request. {ex.Message}";
                return BadRequest(_response);
            }
        }


        [Authorize]
        [HttpPost]
		[Route("Add")]
		[ProducesResponseType(typeof(string), 500)]
		public async Task<IActionResult> Add(FixDto fixDto)
		{
			try
			{
				_fixRepository.InsertOne(_mapper.Map<Fix>(fixDto));
				await _categoryService.Add(fixDto.Category);
				fixDto.Tags.ToList().ForEach(async tag => { await _tagService.Add(tag); });
				await _authorService.Add(fixDto.Author);
				_response.Result = fixDto;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message;
				return BadRequest(_response);
			}
		}

		[Authorize]
		[HttpDelete]
		[Route("Delete/{id}")]
		public IActionResult Delete(string id)
		{
            var fix = _mapper.Map<FixDto>(_fixRepository.FindById(id));

            if (fix == null)
            {
                return NotFound(); // Fix not found
            }

            // Check if the authorized user is the fix's author
            if (!User.HasClaim(claim => claim.Type == "name" && claim.Value.ToLower() == fix.Author.ToLower()))
            {
                return BadRequest("You are not authorized to delete this fix.");
            }

            try
			{
				_fixRepository.DeleteById(id);
				_response.Result = id;
				return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message;
				return BadRequest(_response);
			}
		}

		[Authorize]
		[HttpPut]
		[Route("Update")]
		public async Task<IActionResult> Update(FixDto fixDto)
		{
            var fixDB = _mapper.Map<FixDto>(_fixRepository.FindById(fixDto.Id));

            if (fixDB == null)
            {
                return NotFound(); // Fix not found
            }

            // Check if the authorized user is the fix's author
            if (!User.HasClaim(claim => claim.Type == "name" && claim.Value.ToLower() == fixDB.Author.ToLower()))
            {
                return BadRequest("You are not authorized to update this fix.");
            }

            try
			{
				var fix = _mapper.Map<Fix>(fixDto);
				_fixRepository.UpdateOne(f => f.Id.Equals(fixDto.Id), fix);
                await _categoryService.Add(fixDto.Category);
                fixDto.Tags.ToList().ForEach(async tag => { await _tagService.Add(tag); });
                await _authorService.Add(fixDto.Author);
                _response.Result =fixDto;
                return Ok(_response);
			}
			catch (Exception ex)
			{
				_response.Message =ex.Message;
				return BadRequest(_response);
			}
		}

	}
}
