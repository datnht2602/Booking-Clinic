using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.DataAccess.EndpointServices
{
    public static class InvoiceEndpoints
    {
        public static void MapInvoiceEndpoints(this IEndpointRouteBuilder routes) 
        {
            routes.MapGet("/getallinvoice", async (IInvoiceRepository repository, string? filterCriteria = null) =>
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
            routes.MapGet("/getinvoice/{id}", async (IInvoiceRepository repository, string id) =>
            {
                Invoice result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
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
            routes.MapPost("/getinvoice", async (IInvoiceRepository repository, [FromBody] Invoice invoice) =>
            {
                if (invoice == null || invoice.Etag != null)
                {
                    return Results.BadRequest();
                }

                var result = await repository.AddAsync(invoice, invoice.Id).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetInvoiceById", new { id = result.Resource.Id }, result.Resource);
            })
                .WithOpenApi();
            routes.MapPut("/getinvoice", async (IInvoiceRepository repository, [FromBody] Invoice invoice) =>
            {
                if (invoice == null || invoice.Etag != null)
                {
                    return Results.BadRequest();
                }

                bool result = await repository.ModifyAsync(invoice, invoice.Etag, invoice.Id).ConfigureAwait(false);
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
            routes.MapDelete("/getinvoice/{id}", async (IInvoiceRepository repository, string id) =>
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
        }
    }
}
