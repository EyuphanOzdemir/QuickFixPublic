using HttpService.Services.IService;
using Infrastructure.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;

namespace HttpService.Services
{
    public class AnalyticsHttpService<T> : BaseHttpService where T:BaseNamedEntity, new()
    {
        private readonly string APIUrl = $"{GlobalValues.AnalyticsAPIBase}/api/";

        private string TypeName => typeof(T).Name;
        public AnalyticsHttpService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : base(httpClientFactory, tokenProvider)
        {
        }

        public async Task<ResponseDto> Add(string name)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = new T() { Name = name },
                Url = $"{APIUrl}{TypeName}"
            }, withBearer:false, basicAuth:true);
        }

        public async Task<ResponseDto> Get(string filter = "")
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.GET,
                Url = $"{APIUrl}{TypeName}?filter={filter}"
            }, withBearer: false, basicAuth: true);
        }

        public async Task<ResponseDto> Merge(List<string> sourceEntities)
        {
            return await SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = sourceEntities,
                Url = $"{APIUrl}{TypeName}/Merge"
            }, withBearer: false, basicAuth: true);
        }
    }
}
