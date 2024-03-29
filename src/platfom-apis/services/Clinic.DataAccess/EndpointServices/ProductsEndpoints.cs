using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Clinic.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.DataAccess.EndpointServices
{
    public static class ProductsEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/getallproducts", async (IProductRepository repository, string? filterCriteria = null) =>
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
            routes.MapGet("/getproduct/{id}", async (IProductRepository repository, string id) =>
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
            
            routes.MapPost("/getproduct", async (IProductRepository repository, [FromBody] Product product) =>
            {
                if (product == null )
                {
                    return Results.BadRequest();
                }

                var result = await repository.AddAsync(product, product.Id).ConfigureAwait(false);
                return Results.Ok(product);
            })
                .WithOpenApi();
            routes.MapPut("/getproduct", async (IProductRepository repository, [FromBody] Product product) =>
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
            routes.MapDelete("/getproduct/{id}", async (IProductRepository repository, string id) =>
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
