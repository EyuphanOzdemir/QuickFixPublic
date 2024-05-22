using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [DisplayName("Email")]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
