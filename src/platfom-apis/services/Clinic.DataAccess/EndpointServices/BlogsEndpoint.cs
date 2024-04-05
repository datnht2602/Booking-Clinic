using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.DataAccess.EndpointServices
{
    public static class BlogsEndpoint
    {
        public static void MapBlogEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/getblogs", async (IBlogRepository repository, string? filterCriteria = null) =>
            {
                IEnumerable<Blog> blog;
                if (string.IsNullOrEmpty(filterCriteria))
                {
                    blog = await repository.GetAsync(string.Empty).ConfigureAwait(false);
                }
                else
                {
                    blog = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
                }

                if (blog.Any())
                {
                    return Results.Ok(blog);
                }
                else
                {
                    return Results.NoContent();
                }
            })
    .WithOpenApi();
            routes.MapGet("/getblog/{id}", async (IBlogRepository repository, string id) =>
            {
                Blog result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
                if (result != null)
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.NoContent();
                }
            })
                .WithOpenApi();

            routes.MapPost("/getblog", async (IBlogRepository repository, [FromBody] Blog blog) =>
            {
                if (blog == null)
                {
                    return Results.BadRequest();
                }

                var result = await repository.AddAsync(blog, blog.Id).ConfigureAwait(false);
                return Results.Ok(blog);
            })
                .WithOpenApi();
            routes.MapPut("/getblog", async (IBlogRepository repository, [FromBody] Blog blog) =>
            {
                if (blog == null || blog.Etag != null)
                {
                    return Results.BadRequest();
                }

                bool result = await repository.ModifyAsync(blog, blog.Etag, blog.Id).ConfigureAwait(false);
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
            routes.MapDelete("/getblog/{id}", async (IBlogRepository repository, string id) =>
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
