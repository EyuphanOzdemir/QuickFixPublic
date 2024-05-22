using Infrastructure;
using Infrastructure.Dto;
using Infrastructure.Models.Dto;

namespace QuickFixWeb.Services.IService
{
    public interface IFixService
    {
        Task<ResponseDto> ListAllAsync();
        Task<ResponseDto> Find(string id);
        Task<ResponseDto> Add(FixDto fixDto);
        Task<ResponseDto> Delete(string id);
        Task<ResponseDto> Update(FixDto fixDto);
        Task<ResponseDto> Search(SearchFixParams searchFixParams);
    }
}
