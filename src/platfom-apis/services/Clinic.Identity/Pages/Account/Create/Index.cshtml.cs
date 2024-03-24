using System.Security.Claims;
using Clinic.Identity;
using Clinic.Identity.Data;
using Clinic.Identity.Models;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapp.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly IIdentityServerInteractionService _interaction;

    [BindProperty]
    public InputModel Input { get; set; }
        
    public Index(
        IIdentityServerInteractionService interaction,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
    }

    public IActionResult OnGet(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }
        
    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if ( await _userManager.FindByEmailAsync(Input.Email) != null)
        {
            ModelState.AddModelError("Input.Email", "Email existed");
        }

        if (ModelState.IsValid)
        {
            var createUser = new ApplicationUser()
            {
                UserName = Input.Email,
                Email = Input.Email,
                EmailConfirmed = true,
                PhoneNumber = Input.Phone,
                Name = Input.Name,
            };
              _userManager.CreateAsync(createUser, Input.Password).GetAwaiter().GetResult();
              _userManager.AddToRoleAsync(createUser, SD.PATIENT).GetAwaiter().GetResult();

            var temp1 = _userManager.AddClaimsAsync(createUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, createUser.Name),
                new Claim(JwtClaimTypes.Role, SD.PATIENT),
                new Claim("dob", createUser.DateOfBirth.ToString())
            }).Result;
            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(createUser.Id )
            {
                DisplayName = createUser.Name
            };

            await HttpContext.SignInAsync(isuser);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(Input.ReturnUrl);
            }

            // request for a local page
            if (Url.IsLocalUrl(Input.ReturnUrl))
            {
                return Redirect(Input.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(Input.ReturnUrl))
            {
                return Redirect("~/");
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }
        }

        return Page();
    }
}