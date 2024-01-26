using Clinic.Data.Store.Contracts;
using Microsoft.AspNetCore.Mvc;
using UserClinic = Clinic.Data.Models.User;

namespace Clinic.DataAccess.EndpointServices
{
    public static class UserEndpoint
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/getallusers", async (IUserRepository repository, string? filterCriteria = null) =>
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
            routes.MapGet("/getuser/{id}", async (IUserRepository repository, string id) =>
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
            routes.MapPost("/getuser", async (IUserRepository repository, [FromBody] UserClinic user) =>
            {
                if (user == null || user.Etag != null)
                {
                    return Results.BadRequest();
                }

                var result = await repository.AddAsync(user, user.Id).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetUserById", new { id = result.Resource.Id }, result.Resource);
            })
                .WithOpenApi();
            routes.MapPut("/getuser", async (IUserRepository repository, [FromBody] UserClinic user) =>
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
            routes.MapDelete("/getuser/{id}", async (IUserRepository repository, string id) =>
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
