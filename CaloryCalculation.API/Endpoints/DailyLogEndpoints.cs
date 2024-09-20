using System.Security.Claims;
using CaloryCalculation.Application.Commands.DailyLogs;
using CaloryCalculation.Application.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalculation.API.Endpoints
{
    public static class DailyLogEndpoints
    {
        public static IEndpointRouteBuilder MapDailyLogEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/dailylog");
            
            group.RequireAuthorization(builder =>
            {
                builder.RequireAuthenticatedUser();
                builder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            });
            
            group.MapPostDailyLogEndpoint();

            return routes;
        }

        private static void MapPostDailyLogEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/add-product", async ([FromBody]AddProductToDailyLogCommand command, ClaimsPrincipal user, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserIdByClaim();

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                command.DTO.UserId = int.Parse(userId);
                
                await mediator.Send(command, cancellationToken);
                return Results.Ok();
            });
        }
    }
}
