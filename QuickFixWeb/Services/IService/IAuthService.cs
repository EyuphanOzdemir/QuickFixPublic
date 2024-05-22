using Infrastructure.Dto;

namespace QuickFixWeb.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto> CheckEmail(string email);
        Task<ResponseDto> CheckName(string name);
    }
}
