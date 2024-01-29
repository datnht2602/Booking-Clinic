using Microsoft.AspNetCore.Identity;

namespace Clinic.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public long DateOfBirth { get; set; }
    }
}
