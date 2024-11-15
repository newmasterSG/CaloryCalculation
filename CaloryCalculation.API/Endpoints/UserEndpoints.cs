using System.Security.Claims;
using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.Helpers;
using CaloryCalculation.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalculation.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/user");
        
        group.RequireAuthorization(builder =>
        {
            builder.RequireAuthenticatedUser();
            builder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        });group.RequireAuthorization();
        
        group.MapPatchUserEndpoint();

        return routes;
    }

    private static void MapPatchUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapPatch("/", async ([FromBody]UpdateUserCommand command, ClaimsPrincipal user, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserIdByClaim();

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            command.upd.UserId = int.Parse(userId);
            
            var result = await mediator.Send(command, cancellationToken);
            return Results.Ok(result);
        });
    }
    
    private static void MapGetUserEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/info", async (ClaimsPrincipal user, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = user.GetUserIdByClaim();

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            if (!int.TryParse(userId, out int id))
            {
                return Results.Problem("Not correct type of id");
            }

            var query = new GetUserInformationById(id);
            
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        });
    }
}