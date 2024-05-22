using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Dto
{
    public class PaymentRequestDto
    {
        public Product Product { get; set; }
        public string SucesssUrl { get; set; }
        public string FailureUrl { get; set; }

        public PaymentRequestDto()
        {
            
        }
        public PaymentRequestDto(Product product, string successUrl, string failureUrl) 
        { 
            Product = product;
            FailureUrl = failureUrl;
            SucesssUrl = successUrl;
        }
    }
}
