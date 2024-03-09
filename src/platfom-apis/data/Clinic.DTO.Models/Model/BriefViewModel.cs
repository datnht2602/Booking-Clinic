namespace Clinic.DTO.Models
{
    public class BriefViewModel
    {

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Clinic Number is required")]
        public string ClinicNumber { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "HealthInsuranceCode is required")]
         public string HealthInsuranceCode { get; set; }
         [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string Email { get; set; }
    }
}