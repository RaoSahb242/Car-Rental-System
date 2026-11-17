using Microsoft.AspNetCore.Identity;

namespace Car_Rental_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}