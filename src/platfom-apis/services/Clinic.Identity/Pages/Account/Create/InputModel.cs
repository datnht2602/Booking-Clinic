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
    [StringLength(100, ErrorMessage = "{0} from {2} to {1} length.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Confirm password are not same with password")]
    public string ConfirmPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} from {2} to {1} length.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name="Name")]
    public string Name {set; get;}
    [Required]
    [Phone]
    public string Phone {set; get;}

    public string ReturnUrl { get; set; }

    public string Button { get; set; }
}