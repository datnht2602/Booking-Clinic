namespace Clinic.DTO.Models
{
    public class BriefViewModel
    {

        [Required(ErrorMessage = "Address is required")]
        public string Address1 { get; set; }


        [Required(ErrorMessage = "District is required")]
        public string District { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "HealthInsuranceCode is required")]
         public string HealthInsuranceCode { get; set; }
         [Required(ErrorMessage = "DateOfBirth is required")]

        public DateTime DateOfBirth { get; set; }
    }
}