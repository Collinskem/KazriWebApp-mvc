using Microsoft.AspNetCore.Identity;

namespace ZamaraWebApp.Models
{
    public class UserViewModel :IdentityUser
    {
        public string? Name { get; set; }
    }
}
