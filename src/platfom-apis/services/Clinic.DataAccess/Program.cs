using Clinic.Data.Store;
using Microsoft.Azure.Cosmos;
using Clinic.DataAccess.Extensions;
using Clinic.Common.Options;
using Clinic.Data.Store.Contracts;

using Microsoft.AspNetCore.Mvc;
using Clinic.Data.Models;
using Clinic.DataAccess.EndpointServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<DatabaseSettingsOptions>(builder.Configuration.GetSection("CosmosDB"));
string accountEndpoint = builder.Configuration.GetValue<string>("CosmosDB:AccountEndpoint");
string authKey = builder.Configuration.GetValue<string>("CosmosDB:AuthKey");
CosmosClientOptions options = new()
{
    SerializerOptions = new() { IgnoreNullValues = true }
};
builder.Services.AddSingleton(s => new CosmosClient(accountEndpoint, authKey, options));
builder.Services.AddRepositories();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapInvoiceEndpoints();

app.MapBookingEndpoints();

app.MapProductEndpoints();

app.MapUserEndpoints();

app.Run();

