
using Clinic.Caching;
using Clinic.Caching.Interfaces;
using Clinic.Common.Middlewares;
using Clinic.Common.Options;
using Clinic.Coupon;
using Clinic.Coupon.Contract;
using Clinic.Coupon.Services;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<ICouponService, CouponService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddScoped<ICouponService, CouponService>();
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
app.MapGet("/getcoupon/{code}", async (string code,[FromServices] ICouponService couponService) =>
    {
        return await couponService.GetCouponByCode(code).ConfigureAwait(false) is ResponseDto coupon ? Results.Ok(coupon) : Results.NotFound();  
    })
    .WithOpenApi();
app.MapGet("/getlistcoupon", async ([FromServices] ICouponService couponService,[FromQuery]string? filterCriteria= null) =>
    {
        return await couponService.GetCoupons(filterCriteria).ConfigureAwait(false) is ResponseDto product ? Results.Ok(product) : Results.NotFound();
    })
    .WithName("GetProduct")
    .WithOpenApi();
app.MapGet("/removecoupon/{id}", async (string id, [FromServices] ICouponService couponService) =>
    {
        return await couponService.RemoveCoupon(id).ConfigureAwait(false) is ResponseDto product ? Results.Ok(product) : Results.NotFound();
    })
    .WithOpenApi();
app.MapGet("/getcoupons/{id}", async (string id, [FromServices] ICouponService couponService) =>
    {
        return await couponService.GetCouponByIdASync(id).ConfigureAwait(false) is ResponseDto product ? Results.Ok(product) : Results.NotFound();
    })
    .WithOpenApi();
app.MapPost("/getcoupons",async (Coupon coupon, ICouponService couponService) =>{
        var result = await couponService.AddCouponAsync(coupon).ConfigureAwait(false);
        return Results.Ok(result);
    })
    .WithOpenApi();
app.MapPut("/getcoupons",async (Coupon coupon, ICouponService couponService) =>{
        if (coupon == null  || coupon.Id == null)
        {
            return Results.BadRequest();
        }
        return await couponService.UpdateCouponAsync(coupon) is ResponseDto productResult  ? Results.Ok(productResult) : Results.NotFound();
    })
    .WithOpenApi();
app.MapDelete("/getcoupons/{id}",async (string id, ICouponService couponService) =>{
        return await couponService.DeleteCouponAsync(id) is ResponseDto productResult ? Results.Ok(productResult) : Results.NotFound();
    })
    .WithOpenApi();
app.Run();
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