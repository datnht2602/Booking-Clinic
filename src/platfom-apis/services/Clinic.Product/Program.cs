using System.ComponentModel.DataAnnotations;
using System.Net;
using Clinic.Caching;
using Clinic.Caching.Interfaces;
using Clinic.Common.Middlewares;
using Clinic.Common.Options;
using Clinic.Data.Models;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Clinic.Product;
using Clinic.Product.Contracts;
using Clinic.Product.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IProductService, ProductService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddScoped<IProductService, ProductService>();
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

//app.UseAuthorization();
app.MapGet("/getproducts", async ([FromServices] IProductService productService,[FromQuery]string? filterCriteria= null) =>
{
    return await productService.GetProductsAsync(filterCriteria).ConfigureAwait(false) is ResponseDto product ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProduct")
.WithOpenApi();
app.MapGet("/getproduct/{id}", async (string id, [FromServices] IProductService productService) =>
{
    return await productService.GetProductByIdASync(id).ConfigureAwait(false) is ResponseDto product ? Results.Ok(product) : Results.NotFound();
})
.WithOpenApi();
app.MapPost("/getproduct",async (Product product, IProductService productService) =>{
            var result = await productService.AddProductAsync(product).ConfigureAwait(false);
            return Results.Ok(result);
})
.WithOpenApi();
app.MapPut("/getproduct",async (Product product, IProductService productService) =>{
    if (product == null || product.Etag == null || product.Id == null)
            {
                return Results.BadRequest();
            }
   return await productService.UpdateProductAsync(product) is ResponseDto productResult  ? Results.Ok(productResult) : Results.NotFound();
})
.WithOpenApi();
app.MapDelete("/getproduct/{id}",async (string id, IProductService productService) =>{
   return await productService.DeleteProductAsync(id) is ResponseDto productResult ? Results.Ok(productResult) : Results.NotFound();
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