using Clinic.BlazorWebPWA;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<CustomAuthenticationMessageHandler>();
builder.Services.AddHttpClient("api", opt => opt.BaseAddress = new Uri("https://localhost:44396"))
	.AddHttpMessageHandler<CustomAuthenticationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));
builder.Services.AddOidcAuthentication(options =>
{
	options.ProviderOptions.Authority = "https://localhost:44396";
	options.ProviderOptions.ClientId = "magic";
	options.ProviderOptions.ResponseType = "code";
	options.ProviderOptions.DefaultScopes.Add("openid");
	options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("Clinic");
    options.ProviderOptions.PostLogoutRedirectUri = "/";
	options.ProviderOptions.RedirectUri = "authentication/login-callback";
});

await builder.Build().RunAsync();

public class CustomAuthenticationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthenticationMessageHandler(IAccessTokenProvider provider, NavigationManager nav) :base(provider, nav)
    {
		ConfigureHandler(new string[] { "https://localhost:44396" });
    }
}
