using System.Security.Claims;
using CaloryCalculation.Application.Commands.Nutrion;
using CaloryCalculation.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalculation.API.Endpoints;

public static class CalculationEndpoints
{
    public static IEndpointRouteBuilder MapCalculationNutrion(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/nutrion");

        group.RequireAuthorization(builder =>
        {
            builder.RequireAuthenticatedUser();
            builder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        });

        group.MapCalculationMacro();
        
        return routes;
    }
    
    private static void MapCalculationMacro(this RouteGroupBuilder group)
    {
        group.MapGet("/getNutrionByUser", async ([FromServices] IMediator mediator, ClaimsPrincipal user, CancellationToken cancellationToken) =>
        {
            var command = new CalculateNutrionByUserIdQuery(user.GetUserIdByClaim());
            
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(result);
        });
    }
}