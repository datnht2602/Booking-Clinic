using System.Net.Http.Headers;
using System.Reflection;
using BlazorClient.Security;
using Clinic.BlazorWebPWA;
using Clinic.BlazorWebPWA.Services;
using Clinic.BlazorWebPWA.Services.IService;
using Clinic.Common.Options;
using Clinic.Message;
using InvoiceSamurai.Client.Documents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Net.payOS;
using QuestPDF.Drawing;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOptions();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

PayOS payOS = new PayOS(builder.Configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
    builder.Configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
    builder.Configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"));
builder.Services.AddSingleton(payOS);
builder.Services.AddHttpClient(name: "ApiGateway",
    configureClient: options =>
    {
        options.BaseAddress = new Uri(builder.Configuration["ApplicationSettings:ApiGatewayEndpoint"]);
    });
builder.Services.AddHttpClient(name: "OrderMessage",
    configureClient: options =>
    {
        options.BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress);
        options.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue(
                mediaType: "application/json", quality: 1.0));
    });


builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = builder.Configuration["ApplicationSettings:IdentityApiEndpoint"];
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
using (Stream streamBarcode = Assembly
           .GetExecutingAssembly()
           .GetManifestResourceStream(AppFonts.LibreBarcode39Resourcename))
using (Stream streamRoboto = Assembly
           .GetExecutingAssembly()
           .GetManifestResourceStream(AppFonts.RobotoResourcename))
{
    FontManager.RegisterFontType(AppFonts.LibreBarcode39, streamBarcode);
    FontManager.RegisterFontType(AppFonts.Roboto, streamRoboto);
}
await builder.Build().RunAsync();

