using Infrastructure.Dto;
using Infrastructure.Models;

namespace HttpService.Services.IService
{
    public interface IBaseHttpService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true, bool basicAuth = false, string referer = "");
    }
}
