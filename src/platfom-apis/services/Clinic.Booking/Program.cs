using System.ComponentModel.DataAnnotations;
using System.Net;
using Clinic.Booking;
using Clinic.Booking.Contracts;
using Clinic.Booking.Services;
using Clinic.Caching;
using Clinic.Caching.Interfaces;
using Clinic.Common.Middlewares;
using Clinic.Common.Options;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IBookingService, BookingService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddSingleton<IEntitySerializer,EntitySerializer>();
builder.Services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
if(builder.Configuration.GetValue<bool>("ApplicationSettings:Redis")){
    builder.Services.AddStackExchangeRedisCache(options =>{
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });
}else{
    builder.Services.AddDistributedMemoryCache();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.MapGet("/getorders", async ([FromServices] IBookingService bookingService,[FromQuery]string? filterCriteria= null) =>
{
  return await bookingService.GetBookingAsync(filterCriteria).ConfigureAwait(false);  
})
.WithName("GetProductById")
.WithOpenApi();
app.MapGet("/getorder/{id}", async (string id,[FromServices] IBookingService bookingService) =>
{
  return await bookingService.GetBookingByIdAsync(id).ConfigureAwait(false) is BookingDetailsViewModel booking ? Results.Ok(booking) : Results.NotFound();  
})
.WithOpenApi();
app.MapPost("/getorder",async (BookingDetailsViewModel booking, IBookingService bookingService) =>{
     if (booking == null || booking.Etag != null)
            {
                return Results.BadRequest();
            }

            var result = await bookingService.AddBookingAsync(booking).ConfigureAwait(false);
            return Results.Created($"/getorder/{result.Id}", result);
})
.WithOpenApi();
app.MapPut("/getorder",async (BookingDetailsViewModel booking, IBookingService bookingService) =>{
    if (booking == null || booking.Etag == null || booking.Id == null)
            {
                return Results.BadRequest();
            }
   return (await bookingService.UpdateBookingAsync(booking)).StatusCode == HttpStatusCode.Accepted ? Results.Accepted() : Results.NoContent();
})
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
        5, retry => TimeSpan.FromSeconds(Math.Pow(2,retry))
        + TimeSpan.FromMicroseconds(random.Next(0,100)));
    return retryPolicy;
}
