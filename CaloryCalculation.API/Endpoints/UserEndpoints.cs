using System.Security.Claims;
using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.Helpers;
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
}