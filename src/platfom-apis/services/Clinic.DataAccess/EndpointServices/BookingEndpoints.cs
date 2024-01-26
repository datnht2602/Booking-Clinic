using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.DataAccess.EndpointServices
{
    public static class BookingEndpoints
    {
        public static void MapBookingEndpoints(this IEndpointRouteBuilder routes) 
        {
            routes.MapGet("/getallbooking", async (IBookingRepository repository, string? filterCriteria = null) =>
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
            routes.MapGet("/getbooking/{id}", async (IBookingRepository repository, string id) =>
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
            routes.MapPost("/getbooking", async (IBookingRepository repository, [FromBody] Booking booking) =>
            {
                if (booking == null || booking.Etag != null)
                {
                    return Results.BadRequest();
                }

                var result = await repository.AddAsync(booking, booking.Id).ConfigureAwait(false);
                return Results.CreatedAtRoute("GetBookingById", new { id = result.Resource.Id }, result.Resource);
            })
                .WithOpenApi();
            routes.MapPut("/getbooking", async (IBookingRepository repository, [FromBody] Booking booking) =>
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
            routes.MapDelete("/getbooking/{id}", async (IBookingRepository repository, string id) =>
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
