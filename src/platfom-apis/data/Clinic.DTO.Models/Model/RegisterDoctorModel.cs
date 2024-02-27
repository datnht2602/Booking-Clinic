namespace Clinic.DTO.Models;

public class RegisterDoctorModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public long DateOfBirth { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public int Specialization { get; set; }
    
    public int Achievement { get; set; }

    public int WorkingExperience { get; set; }
    
    public string ClinicNum { get; set; }
}