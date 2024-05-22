using Azure.Core;
using HttpService.Services.IService;
using Microsoft.AspNetCore.Http;
using Infrastructure;
using Infrastructure.Dto;
using Infrastructure.Models;
using Infrastructure.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Infrastructure.Utility.GlobalValues;

namespace HttpService.Services
{
    public class BaseHttpService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : IBaseHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true, bool basicAuth=false, string referer ="")
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("PelicanAPI");
                HttpRequestMessage message = new();
                if (requestDto.ContentType == GlobalValues.ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                    //message.Headers.Add("Accept", "*/*");
                }



                if (basicAuth)
                {
                    // Set up Basic Authentication header
                    var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("robespierre:Yarasa13_")));
                    client.DefaultRequestHeaders.Authorization = authValue;
                }
                else
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                if (!string.IsNullOrEmpty(referer))
                {
                    message.Headers.Referrer = new Uri(referer);
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.ContentType == GlobalValues.ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach(var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is FormFile file)
                        {
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                    }
                }

                HttpResponseMessage apiResponse = null;

                message.Method = requestDto.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.DELETE => HttpMethod.Delete,
                    ApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get,
                };
                
                
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
