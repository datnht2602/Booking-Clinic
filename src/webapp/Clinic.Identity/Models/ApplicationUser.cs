using Microsoft.AspNetCore.Identity;

namespace Clinic.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public int? Specialization { get; set; }
        public string? HealthInsuranceCode { get; set; }
        public long DateOfBirth { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
    }
}
