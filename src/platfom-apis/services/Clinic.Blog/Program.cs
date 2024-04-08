using Clinic.Blog.Contracts;
using Clinic.Blog.Services;
using Clinic.Caching;
using Clinic.Caching.Interfaces;
using Clinic.Common.Middlewares;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Polly;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IBlogService, BlogService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddSingleton<IEntitySerializer, EntitySerializer>();
builder.Services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
if (builder.Configuration.GetValue<bool>("ApplicationSettings:Redis"))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });
}
else
{
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

//app.UseAuthorization();
app.MapGet("/getblogs", async ([FromServices] IBlogService blogService, [FromQuery] string? filterCriteria = null) =>
{
    return await blogService.GetBlogsAsync(filterCriteria).ConfigureAwait(false) is ResponseDto blog ? Results.Ok(blog) : Results.NotFound();
})
.WithName("GetProduct")
.WithOpenApi();
app.MapGet("/getblog/{id}", async (string id, [FromServices] IBlogService blogService) =>
{
    return await blogService.GetBlogByIdASync(id).ConfigureAwait(false) is ResponseDto blog ? Results.Ok(blog) : Results.NotFound();
})
.WithOpenApi();
app.MapPost("/getblog", async (Blog blog, IBlogService blogService) =>
{
    var result = await blogService.AddBlogAsync(blog).ConfigureAwait(false);
    return Results.Ok(result);
})
.WithOpenApi();
app.MapPut("/getblog", async (Blog blog, IBlogService blogService) =>
{
    if (blog == null || blog.Etag == null || blog.Id == null)
    {
        return Results.BadRequest();
    }
    return await blogService.UpdateBlogAsync(blog) is ResponseDto Blog ? Results.Ok(Blog) : Results.NotFound();
})
.WithOpenApi();
app.MapDelete("/getblog/{id}", async (string id, IBlogService blogService) =>
{
    return await blogService.DeleteBlogAsync(id) is ResponseDto blog ? Results.Ok(blog) : Results.NotFound();
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
        5, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
        + TimeSpan.FromMicroseconds(random.Next(0, 100)));
    return retryPolicy;
}