namespace Clinic.DTO.Models.Dto;

public class ChangePasswordDto
{
    public string Id { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} from {2} to {1} words.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
}