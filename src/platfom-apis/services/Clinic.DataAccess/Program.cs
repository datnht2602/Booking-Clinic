using Clinic.Data.Store;
using Microsoft.Azure.Cosmos;
using Clinic.DataAccess.Extensions;
using Clinic.Common.Options;
using Clinic.Data.Store.Contracts;
using UserClinic = Clinic.Data.Models.User;
using Microsoft.AspNetCore.Mvc;
using Clinic.Data.Models;
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

//app.UseAuthorization();
#region Invoice
app.MapGet("/getallinvoice", async (IInvoiceRepository repository, string? filterCriteria = null) =>
{
    IEnumerable<Invoice> invoice;
    if (string.IsNullOrEmpty(filterCriteria))
    {
        invoice = await repository.GetAsync(string.Empty).ConfigureAwait(false);
    }
    else
    {
        invoice = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
    }

    if (invoice.Any())
    {
        return Results.Ok(invoice);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapGet("/getinvoice/{id}", async (IInvoiceRepository repository,string id) =>
{
    Invoice result = await repository.GetByIdAsync(id,id).ConfigureAwait(false);
    if (result != null)
    {
        return Results.Ok(result);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithName("GetInvoiceById")
    .WithOpenApi();
app.MapPost("/getinvoice", async (IInvoiceRepository repository, [FromBody] Invoice invoice) =>
{
    if( invoice == null || invoice.Etag != null)
    {
        return Results.BadRequest();
    }

    var result = await repository.AddAsync(invoice, invoice.Id).ConfigureAwait(false);
    return Results.CreatedAtRoute("GetInvoiceById", new { id = result.Resource.Id }, result.Resource);
})
    .WithOpenApi();
app.MapPut("/getinvoice", async (IInvoiceRepository repository, [FromBody] Invoice invoice) =>
{
    if (invoice == null || invoice.Etag != null)
    {
        return Results.BadRequest();
    }

    bool result = await repository.ModifyAsync(invoice,invoice.Etag ,invoice.Id).ConfigureAwait(false);
    if(result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapDelete("/getinvoice/{id}", async (IInvoiceRepository repository, string id) =>
{

    bool result = await repository.RemoveAsync(id,id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
#endregion
#region Booking
app.MapGet("/getallbooking", async (IBookingRepository repository, string? filterCriteria = null) =>
{
    IEnumerable<Booking> booking;
    if (string.IsNullOrEmpty(filterCriteria))
    {
        booking = await repository.GetAsync(string.Empty).ConfigureAwait(false);
    }
    else
    {
        booking = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
    }

    if (booking.Any())
    {
        return Results.Ok(booking);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapGet("/getbooking/{id}", async (IBookingRepository repository, string id) =>
{
    Booking result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
    if (result != null)
    {
        return Results.Ok(result);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithName("GetBookingById")
    .WithOpenApi();
app.MapPost("/getbooking", async (IBookingRepository repository, [FromBody] Booking booking) =>
{
    if (booking == null || booking.Etag != null)
    {
        return Results.BadRequest();
    }

    var result = await repository.AddAsync(booking, booking.Id).ConfigureAwait(false);
    return Results.CreatedAtRoute("GetBookingById", new { id = result.Resource.Id }, result.Resource);
})
    .WithOpenApi();
app.MapPut("/getbooking", async (IBookingRepository repository, [FromBody] Booking booking) =>
{
    if (booking == null || booking.Etag != null)
    {
        return Results.BadRequest();
    }

    bool result = await repository.ModifyAsync(booking, booking.Etag, booking.Id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapDelete("/getbooking/{id}", async (IBookingRepository repository, string id) =>
{

    bool result = await repository.RemoveAsync(id, id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
#endregion
#region Products
app.MapGet("/getallproducts", async (IProductRepository repository, string? filterCriteria = null) =>
{
    IEnumerable<Product> product;
    if (string.IsNullOrEmpty(filterCriteria))
    {
        product = await repository.GetAsync(string.Empty).ConfigureAwait(false);
    }
    else
    {
        product = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
    }

    if (product.Any())
    {
        return Results.Ok(product);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapGet("/getproduct/{id}", async (IProductRepository repository, string id) =>
{
    Product result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
    if (result != null)
    {
        return Results.Ok(result);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithName("GetProductById")
    .WithOpenApi();
app.MapPost("/getproduct", async (IProductRepository repository, [FromBody] Product product) =>
{
    if (product == null || product.Etag != null)
    {
        return Results.BadRequest();
    }

    var result = await repository.AddAsync(product, product.Id).ConfigureAwait(false);
    return Results.CreatedAtRoute("GetProductById", new { id = result.Resource.Id }, result.Resource);
})
    .WithOpenApi();
app.MapPut("/getproduct", async (IProductRepository repository, [FromBody] Product product) =>
{
    if (product == null || product.Etag != null)
    {
        return Results.BadRequest();
    }

    bool result = await repository.ModifyAsync(product, product.Etag, product.Id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapDelete("/getproduct/{id}", async (IProductRepository repository, string id) =>
{

    bool result = await repository.RemoveAsync(id, id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
#endregion
#region Users
app.MapGet("/getallusers", async (IUserRepository repository, string? filterCriteria = null) =>
{
    IEnumerable<UserClinic> user;
    if (string.IsNullOrEmpty(filterCriteria))
    {
        user = await repository.GetAsync(string.Empty).ConfigureAwait(false);
    }
    else
    {
        user = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
    }

    if (user.Any())
    {
        return Results.Ok(user);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapGet("/getuser/{id}", async (IUserRepository repository, string id) =>
{
    UserClinic result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
    if (result != null)
    {
        return Results.Ok(result);
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithName("GetUserById")
    .WithOpenApi();
app.MapPost("/getuser", async (IUserRepository repository, [FromBody] UserClinic user) =>
{
    if (user == null || user.Etag != null)
    {
        return Results.BadRequest();
    }

    var result = await repository.AddAsync(user, user.Id).ConfigureAwait(false);
    return Results.CreatedAtRoute("GetUserById", new { id = result.Resource.Id }, result.Resource);
})
    .WithOpenApi();
app.MapPut("/getuser", async (IUserRepository repository, [FromBody] UserClinic user) =>
{
    if (user == null || user.Etag != null)
    {
        return Results.BadRequest();
    }

    bool result = await repository.ModifyAsync(user, user.Etag, user.Id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
app.MapDelete("/getuser/{id}", async (IUserRepository repository, string id) =>
{

    bool result = await repository.RemoveAsync(id, id).ConfigureAwait(false);
    if (result)
    {
        return Results.Accepted();
    }
    else
    {
        return Results.NoContent();
    }
})
    .WithOpenApi();
#endregion
app.Run();

