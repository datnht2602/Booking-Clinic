// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;
using Clinic.Common.Options;
using Microsoft.Extensions.Options;

namespace webapp.Pages.Login;

public class InputModel
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
        
    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; } = "https://localhost:7072";

    public string Button { get; set; }
}