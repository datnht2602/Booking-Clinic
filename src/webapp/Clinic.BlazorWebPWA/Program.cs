using BlazorClient.Security;
using Clinic.BlazorWebPWA;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("api")
               .AddHttpMessageHandler(sp =>
               {
                   var handler = sp.GetService<AuthorizationMessageHandler>()
                       .ConfigureHandler(
                           authorizedUrls: new[] { "https://localhost:7244" },
                           scopes: new[] { "Clinic" });

                   return handler;
               });

builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));
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
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

await builder.Build().RunAsync();

