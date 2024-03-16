using System.Net.Http.Headers;
using BlazorClient.Security;
using Clinic.BlazorWebPWA;
using Clinic.BlazorWebPWA.Services;
using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Common.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOptions();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient(name: "ApiGateway",
    configureClient: options =>
    {
        options.BaseAddress = new Uri("https://localhost:7024/");
        options.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                mediaType: "application/json", quality: 1.0));
    });
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://localhost:7268";
    options.ProviderOptions.ClientId = "Clinic";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
    options.ProviderOptions.DefaultScopes.Add("Clinic");
    options.ProviderOptions.PostLogoutRedirectUri = "/";
    options.ProviderOptions.RedirectUri = "authentication/login-callback";
    options.UserOptions.RoleClaim = "role";
});

await builder.Build().RunAsync();

