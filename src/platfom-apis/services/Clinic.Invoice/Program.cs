using Clinic.Caching;
using Clinic.Caching.Interfaces;
using Clinic.Common.Middlewares;
using Clinic.Common.Options;
using Clinic.DTO.Models;
using Clinic.Invoice;
using Clinic.Invoice.Contracts;
using Clinic.Invoice.Extension;
using Clinic.Invoice.Message;
using Clinic.Invoice.Services;
using Clinic.Message;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddHttpClient<IInvoiceService, InvoiceService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    .AddPolicyHandler(RetryPolicy()) // Retry policy
    .AddPolicyHandler(CircuitBreakerPolicy());
builder.Services.AddSingleton<IInvoiceService, InvoiceService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddSingleton<IEntitySerializer,EntitySerializer>();
builder.Services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
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

app.MapGet("/getinvoice/{id}", async (string id,[FromServices] IInvoiceService invoiceService) =>
{
  return await invoiceService.GetInvoiceByIdAsync(id) is InvoiceDetailsViewModel invoice ? Results.Ok(invoice) : Results.NotFound();  
})
.WithName("GetInvoiceById")
.WithOpenApi();
app.MapPost("/getinvoice",async (InvoiceDetailsViewModel invoice, IInvoiceService invoiceService) =>{
     if (invoice == null || invoice.Etag != null)
            {
                return Results.BadRequest();
            }

            var result = await invoiceService.AddInvoiceAsync(invoice).ConfigureAwait(false);
            return Results.Created($"/getinvoice/{result.Id}", result);
})
.WithOpenApi();
app.UseAzureServiceBusConsumer();
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