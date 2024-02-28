using Clinic.ApiGateway.Contracts;
using Clinic.ApiGateway.Services;
using Clinic.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Polly.Extensions.Http;
using Polly;
using Clinic.Caching.Interfaces;
using Clinic.Caching;
using Clinic.Common.Middlewares;
using Clinic.ApiGateway;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IClinicService, ClinicService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IClinicService, ClinicService>();
builder.Services.AddSingleton<IEntitySerializer, EntitySerializer>();
builder.Services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:7268";
        options.Audience = "Clinic";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name"
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();

app.MapGet("/getdoctors", async ([FromServices] IClinicService clinicService, [FromQuery] string? filterCriteria = null) =>
{
  return await clinicService.GetDoctorsAsync(filterCriteria).ConfigureAwait(false);
    
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
static IAsyncPolicy<HttpResponseMessage> RetryPolicy()
{
    Random random = new();
    var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(
        5, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
        + TimeSpan.FromMicroseconds(random.Next(0, 100)));
    return retryPolicy;
}

