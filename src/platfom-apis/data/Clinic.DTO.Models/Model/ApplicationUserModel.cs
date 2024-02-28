namespace Clinic.DTO.Models;

public class ApplicationUserModel
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? Name { get; set; }
    
    public string? Detail { get; set; }
    
    public long DateOfBirth { get; set; }
    
    [StringLength(1024)]
    public string? ImageUrl { get; set; }
    
    [StringLength(1024)]
    public string? Introduction { get; set; }
    
    public double AverageRating { get; set; }
    public int ExperienceYear { get; set; }
    public string Title { get; set; }
    public int Specialization { get; set; }
    public string ClinicNum { get; set; }
}