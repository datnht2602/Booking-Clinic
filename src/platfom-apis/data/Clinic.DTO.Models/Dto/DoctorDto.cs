using Clinic.Data.Models;

namespace Clinic.DTO.Models.Dto;

public class DoctorDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Title { get; set; }
    public int Specialization { get; set; }
    public string ClinicNum { get; set; }
    public string ImageUrl { get; set; }
    public string Introduction { get; set; }
    public int Rating { get; set; }
}