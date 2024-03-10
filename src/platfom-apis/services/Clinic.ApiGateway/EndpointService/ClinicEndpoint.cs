using Clinic.ApiGateway.Contracts;
using Clinic.DTO.Models;
using Clinic.DTO.Models.Dto;
using Microsoft.AspNetCore.Authorization;
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
        routes.MapGet("/getbooking", [Authorize]async ([FromServices] IClinicService clinicService, [FromQuery] string userId) =>
            {
                var booking = await clinicService.GetBookingDetail(userId).ConfigureAwait(false);
                ResponseDto result = new();
                result.Result = booking;
                return result;
    
            })
            .WithOpenApi();
        routes.MapPost("/upsertbooking", [Authorize]async ([FromBody]BookingDetailsViewModel createModel,[FromServices]IClinicService clinicService) =>
            {
                var booking = await clinicService.CreateOrUpdateBooking(createModel).ConfigureAwait(false);
                ResponseDto result = new();
                result.Result = booking;
                return result;
    
            })
            .WithOpenApi();
        routes.MapGet("/getbookingbyid", [Authorize]async (string bookingId,[FromServices]IClinicService clinicService) =>
            {
                var booking = await clinicService.GetBookingByIdAsync(bookingId).ConfigureAwait(false);
                ResponseDto result = new();
                result.Result = booking;
                return result;
    
            })
            .WithOpenApi();
    }
}