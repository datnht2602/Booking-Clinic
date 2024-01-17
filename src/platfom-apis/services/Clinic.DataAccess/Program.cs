using Clinic.Data.Store;
using Microsoft.Azure.Cosmos;
using Clinic.DataAccess.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.Configure<DatabaseSettingsOptions>(builder.Configuration.GetSection("CosmosDB"));
string accountEndpoint = builder.Configuration.GetValue<string>("CosmosDB:AccountEndpoint");
string authKey = builder.Configuration.GetValue<string>("CosmosDB:AuthKey");
builder.Services.AddSingleton(s => new CosmosClient(accountEndpoint, authKey));
builder.Services.AddRepositories();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

