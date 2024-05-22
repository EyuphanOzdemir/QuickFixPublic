using Azure;
using Infrastructure.Dto;
using Infrastructure.Models.Dto;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class CheckoutController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private static string clientURL = string.Empty;

    public CheckoutController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout([FromBody] PaymentRequestDto paymentRequestDto)
    {

        var response = new ResponseDto();
        try
        { 
            var sessionUrl = await CheckOut(paymentRequestDto);
           
            var pubKey = _configuration["Stripe:PubKey"];

            var checkoutOrderResponse = new PaymentResponseDto()
            {
                SessionUrl = sessionUrl,
                PubKey = pubKey
            };

            response = new ResponseDto() { IsSuccess = true, Message="OK", Result= checkoutOrderResponse };
        }
        catch (Exception ex) 
        {
            response = new ResponseDto() { IsSuccess = true, Message = $"Error:{ex.Message}", Result = "" };
        }

        return Ok(response);
    }

    [NonAction]
    public async Task<string> CheckOut(PaymentRequestDto paymentRequestDto)
    {
        // Create a payment flow from the items in the cart.
        // Gets sent to Stripe API.
        var options = new SessionCreateOptions
        {
            // Stripe calls the URLs below when certain checkout events happen such as success and failure.
            SuccessUrl = paymentRequestDto.SucesssUrl, // Customer paid.
            CancelUrl = paymentRequestDto.FailureUrl,  // Checkout cancelled.
            PaymentMethodTypes = new List<string> // Only card available in test mode?
            {
                "card"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = paymentRequestDto.Product.Price, // Price is in USD cents.
                        Currency = "USD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = paymentRequestDto.Product.Title,
                            Description = paymentRequestDto.Product.Description
                            //Images = new List<string> { product.ImageUrl }
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment" // One-time payment. Stripe supports recurring 'subscription' payments.
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }

    [HttpGet("success")]
    // Automatic query parameter handling from ASP.NET.
    // Example URL: https://localhost:7051/checkout/success?sessionId=si_123123123123
    public ActionResult CheckoutSuccess(string sessionId)
    {
        var sessionService = new SessionService();
        var session = sessionService.Get(sessionId);

        // Here you can save order and customer details to your database.
        var total = session.AmountTotal.Value;
        var customerEmail = session.CustomerDetails.Email;

        return Redirect(clientURL + "success");
    }
}
