using HttpService.Services.IService;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure;
using Infrastructure.Utility;
using Infrastructure.Dto;
using Newtonsoft.Json;
using System;
using Infrastructure.Models.Dto;

namespace QuickFixWeb.Pages.Payment
{
    public class StartModel(IBaseHttpService baseHttpService) : PageModel
    {
        public IBaseHttpService _baseHttpService { get; } = baseHttpService;

        public async Task<IActionResult> OnGet([FromServices] IServiceProvider sp)
        {
            var server = sp.GetRequiredService<IServer>();

            var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

            string thisApiUrl = null;

            if (serverAddressesFeature is not null)
            {
                thisApiUrl = serverAddressesFeature.Addresses.FirstOrDefault();
            }

            var response = await _baseHttpService.SendAsync
             (
                new Infrastructure.Dto.RequestDto()
                {
                    ApiType = GlobalValues.ApiType.POST,
                    Data =new PaymentRequestDto(new Product() { Description = "Donation", Price = 100, Title = "Donation" },
                                                $"{thisApiUrl}/Payment/Success",
                                                $"{thisApiUrl}/Payment/Failure"),
                    Url = GlobalValues.StripeAPIBase + "/api/checkout/checkout"
                }, false, false
             );


            if (response.IsSuccess)
            {
                var paymentResponse = JsonConvert.DeserializeObject<PaymentResponseDto>(response.Result.ToString());
                var stripeCheckoutUrl = paymentResponse.SessionUrl;
                Response.Headers.Append("Location", stripeCheckoutUrl);
                return new StatusCodeResult(303);
            }
            else
            {
                ViewData["Message"] = response.Message;
                return Page();
            }
        }
    }
}
