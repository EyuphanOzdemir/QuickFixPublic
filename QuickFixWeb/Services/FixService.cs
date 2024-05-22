using HttpService.Services.IService;
using HttpService.Utility;
using Infrastructure;
using Infrastructure.Dto;
using Infrastructure.Models.Dto;
using Infrastructure.Utility;
using QuickFixWeb.Services.IService;

namespace QuickFixWeb.Services
{
    public class FixService(IBaseHttpService baseService) : IFixService
    {   
        private readonly IBaseHttpService _baseService = baseService;

        public async Task<ResponseDto> Add(FixDto fixDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = fixDto,
                Url = GlobalValues.FixAPIBase + "/api/fix/add"
            }); 
        }

        public async Task<ResponseDto> Delete(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.DELETE,
                Url = GlobalValues.FixAPIBase + "/api/fix/delete/" + id
            });
        }

        public async Task<ResponseDto> Find(string id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.GET,
                Url = GlobalValues.FixAPIBase + "/api/fix/find/" + id
            });
        }

        public async Task<ResponseDto> ListAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
                                                  {
                                                      ApiType = GlobalValues.ApiType.GET,
                                                      Url = GlobalValues.FixAPIBase + "/api/fix"
                                                  });
        }

        public async Task<ResponseDto> Update(FixDto fixDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.PUT,
                Data = fixDto,
                Url = GlobalValues.FixAPIBase + "/api/fix/update"
            });
        }

        public async Task<ResponseDto> Search(SearchFixParams searchFixParams)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.GET,
                Url = $"{GlobalValues.FixAPIBase}/api/fix/search?{HttpService.Utility.Utilities.GetQueryString(searchFixParams)}"
            }, false, false);
        }
    }
}
