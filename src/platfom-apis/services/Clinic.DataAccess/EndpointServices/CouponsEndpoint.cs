using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.DataAccess.EndpointServices;

public static class CouponsEndpoint
{
    public static void MapCouponEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/getcoupon", async (ICouponRepository repository,[FromQuery] string? code = null ) =>
            {
                Coupon result = await repository.GetByNameCouponAsync(code).ConfigureAwait(false);
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
        routes.MapGet("/getallcoupons", async (ICouponRepository repository, string? filterCriteria = null) =>
        {
            IEnumerable<Coupon> coupon;
            if (string.IsNullOrEmpty(filterCriteria))
            {
                coupon = await repository.GetAsync(string.Empty).ConfigureAwait(false);
            }
            else
            {
                coupon = await repository.GetAsync(filterCriteria).ConfigureAwait(false);
            }

            if (coupon.Any())
            {
                return Results.Ok(coupon);
            }
            else
            {
                return Results.NoContent();
            }
        })
    .WithOpenApi();
        routes.MapGet("/getcoupons/{id}", async (ICouponRepository repository, string id) =>
        {
            Coupon result = await repository.GetByIdAsync(id, id).ConfigureAwait(false);
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

        routes.MapPost("/getcoupons", async (ICouponRepository repository, [FromBody] Coupon coupon) =>
        {
            if (coupon == null)
            {
                return Results.BadRequest();
            }

            var result = await repository.AddAsync(coupon, coupon.Id).ConfigureAwait(false);
            return Results.Ok(coupon);
        })
            .WithOpenApi();
        routes.MapPut("/getcoupons", async (ICouponRepository repository, [FromBody] Coupon coupon) =>
        {
            if (coupon == null )
            {
                return Results.BadRequest();
            }

            bool result = await repository.ModifyAsync(coupon, coupon.Etag, coupon.Id).ConfigureAwait(false);
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
        routes.MapDelete("/getcoupons/{id}", async (ICouponRepository repository, string id) =>
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