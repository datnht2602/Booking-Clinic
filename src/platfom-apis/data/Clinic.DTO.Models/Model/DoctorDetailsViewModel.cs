using Clinic.DTO.Models.Dto;

namespace Clinic.DTO.Models
{
    public class DoctorDetailsViewModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public int ExperienceYear { get; set; }
        public long DateOfBirth { get; set; }
        public string Title { get; set; }
        public int Specialization { get; set; }
        public string ClinicNum { get; set; }
        public double AverageRating { get; set; }
        public string Introduction { get; set; }

        public List<FeedBackDto> FeedBack { get; set; }

    }
}
