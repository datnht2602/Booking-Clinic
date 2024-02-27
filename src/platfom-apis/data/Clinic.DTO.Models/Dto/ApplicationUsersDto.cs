namespace Clinic.DTO.Models.Dto;

public class ApplicationUsersDto
{
    public string? UserName { get; set; }
    
    public string? NormalizedUserName { get; set; }
    
    public string? Email { get; set; }
    
    public string? NormalizedEmail { get; set; }
    
    public bool EmailConfirmed { get; set; }
    
    public string? PasswordHash { get; set; }
    
    public string? SecurityStamp { get; set; }
    
    public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    
    public string? PhoneNumber { get; set; }
    
    public bool PhoneNumberConfirmed { get; set; }
    
    public bool TwoFactorEnabled { get; set; }
    
    public DateTimeOffset? LockoutEnd { get; set; }
    
    public bool LockoutEnabled { get; set; }
    
    public int AccessFailedCount { get; set; }
    
    public string? Name { get; set; }
    
    public string? Detail { get; set; }
    
    public long DateOfBirth { get; set; }
    
    [StringLength(1024)]
    public string? ImageUrl { get; set; }
    
    [StringLength(1024)]
    public string? Introduction { get; set; }
    
    public double AverageRating { get; set; }
}