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
            .WithName("GetCouponById")
            .WithOpenApi();
    }
}