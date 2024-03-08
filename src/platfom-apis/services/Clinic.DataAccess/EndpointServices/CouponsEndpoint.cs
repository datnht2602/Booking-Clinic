using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;

namespace Clinic.DataAccess.EndpointServices;

public static class CouponsEndpoint
{
    public static void MapCouponEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/getcoupon/{code}", async (ICouponRepository repository, string code ) =>
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