using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
