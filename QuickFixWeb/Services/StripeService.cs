using HttpService.Services.IService;
using Infrastructure;
using Infrastructure.Dto;
using Infrastructure.Utility;
using QuickFixWeb.Services.IService;

namespace QuickFixWeb.Services
{
    public class StripeService(IBaseHttpService httpService) : IStripeService
    {
        private IBaseHttpService _httpService { get; } = httpService;

        public async Task<ResponseDto> CheckOut(Product product)
        {
            return await _httpService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = product,
                Url = GlobalValues.StripeAPIBase + "/api/checkout/checkout"
            });
        }
    }
}
