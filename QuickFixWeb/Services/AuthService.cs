using HttpService.Services.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Infrastructure.Dto;
using Infrastructure.Utility;
using QuickFixWeb.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Infrastructure;


namespace QuickFixWeb.Services
{
    public class AuthService(IBaseHttpService baseService) : IAuthService
    {
        private readonly IBaseHttpService _baseService = baseService;
        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = registrationRequestDto,
                Url = GlobalValues.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto> CheckEmail(string email)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.GET,
                Url = GlobalValues.AuthAPIBase + $"/api/auth/checkemail?email={email}"
            }, withBearer: false);
        }

        public async Task<ResponseDto> CheckName(string name)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.GET,
                Url = GlobalValues.AuthAPIBase + $"/api/auth/checkname?name={name}"
            }, withBearer: false);
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = loginRequestDto,
                Url = GlobalValues.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = GlobalValues.ApiType.POST,
                Data = registrationRequestDto,
                Url = GlobalValues.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
