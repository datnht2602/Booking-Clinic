using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Clinic.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public long DateOfBirth { get; set; }
        [StringLength(1024)]
        public string? ImageUrl { get; set; }
        [StringLength(1024)]
        public string? Introduction { get; set; }
        public double AverageRating { get; set; }
    }
}
