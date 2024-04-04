using System.ComponentModel.DataAnnotations;
using System.Text;
using Clinic.Common.Options;
using Clinic.DTO.Models.Dto;
using Clinic.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace webapp.Pages.ForgotPassword;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HttpClient httpClient;
    private readonly IOptions<ApplicationSettings> applicationSettings;
    private const string ContentType = "application/json";
    public Index(UserManager<ApplicationUser> userManager,IHttpClientFactory httpClientFactory,IOptions<ApplicationSettings> applicationSettings)
    {
        _userManager = userManager;
        httpClient = httpClientFactory.CreateClient();
        this.applicationSettings = applicationSettings;
    }
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Input Email Address")]
        public string Email { get; set; }
    }
    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            // Tìm user theo email gửi đến
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // Phát sinh Token để reset password
            // Token sẽ được kèm vào link trong email,
            // link dẫn đến trang /Account/ResetPassword để kiểm tra và đặt lại mật khẩu
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = $"{this.applicationSettings.Value.IdentityApiEndpoint}/Account/ResetPassword?code={code}";
                /*Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);*/
            var mailDto = new SendMailDto()
            {
                Email = Input.Email,
                CallBackUrl = callbackUrl
            };
            using var emailRequest = new StringContent(JsonSerializer.Serialize(mailDto),Encoding.UTF8,ContentType);
            var emailResponse = await httpClient.PostAsync(new Uri($"{this.applicationSettings.Value.EmailEndpoint}/resetpassword"),emailRequest).ConfigureAwait(false);
            // Gửi email

            // Chuyển đến trang thông báo đã gửi mail để reset password
            return RedirectToPage("/Account/Login/Index");
        }

        return Page();
    }

}