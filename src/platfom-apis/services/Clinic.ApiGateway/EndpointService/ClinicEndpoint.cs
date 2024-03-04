using Clinic.ApiGateway.Contracts;
using Clinic.DTO.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.ApiGateway.EndpointService;

public static class ClinicEndpoint
{
    public static void MapEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/getdoctors", async ([FromServices] IClinicService clinicService, [FromQuery] string? filterCriteria = null) =>
            {
                var list = await clinicService.GetDoctorsAsync(filterCriteria).ConfigureAwait(false);
                ResponseDto result = new();
                result.Result = list;
                return result;
    
            }).AllowAnonymous()
            .WithOpenApi();
        routes.MapGet("/getbooking", async ([FromServices] IClinicService clinicService, [FromQuery] string userId) =>
            {
                var list = await clinicService.GetBookingDetail(userId).ConfigureAwait(false);
                ResponseDto result = new();
                result.Result = list;
                return result;
    
            }).AllowAnonymous()
            .WithOpenApi();
    }
}