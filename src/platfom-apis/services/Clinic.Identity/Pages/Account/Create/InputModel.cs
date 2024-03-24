// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace webapp.Pages.Create;

public class InputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu")]
    [Compare("Password", ErrorMessage = "Password not same")]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} from {2} to {1} length.", MinimumLength = 3)]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string Name {set; get;}
    [Required]
    [StringLength(10, MinimumLength = 10,ErrorMessage = "Phone number length is 10")]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string Phone {set; get;}

    public string ReturnUrl { get; set; }

    public string Button { get; set; }
}