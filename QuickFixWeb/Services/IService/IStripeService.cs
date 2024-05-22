using Infrastructure;
using Infrastructure.Dto;

namespace QuickFixWeb.Services.IService
{
    public interface IStripeService
    {
        Task<ResponseDto> CheckOut(Product product);
    }
}
