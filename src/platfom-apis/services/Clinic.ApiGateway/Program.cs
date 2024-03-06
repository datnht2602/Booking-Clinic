using System.Text;
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
using Clinic.ApiGateway.EndpointService;
using Clinic.DTO.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

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
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {

        options.Authority = "https://localhost:7268/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

    });
builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddCors( options => {
    options.AddPolicy("AllowAll", 
        b => b.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic.Services.Apis", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme="oauth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }

    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clinic.Services"));
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapEndpoints();

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

