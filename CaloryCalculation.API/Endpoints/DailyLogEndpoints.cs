using System.Security.Claims;
using CaloryCalculation.Application.Commands.DailyLogs;
using CaloryCalculation.Application.DTOs.DailyLogs;
using CaloryCalculation.Application.Helpers;
using CaloryCalculation.Application.Queries.DailyLog;
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
            group.MapDeleteDailyLogEndpoint();
            group.MapGetDailyLogEndpoint();
            
            return routes;
        } 

        private static void MapGetDailyLogEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/by-user", async (string date, ClaimsPrincipal user, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserIdByClaim();

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
                var parsedDate = DateTime.Parse(date);
                if (parsedDate.Kind == DateTimeKind.Unspecified)
                {
                    parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                }
                
                var Dto = new GetDailyLogUserDTO
                {
                    Date = parsedDate,
                };

                var command = new GetDailyLogByUserQuery(Dto);
                
                command.Dto.UserId = int.Parse(userId);
                
                var dto = await mediator.Send(command, cancellationToken);
                return Results.Ok(dto);
            });
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

        private static void MapDeleteDailyLogEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/delete-product", async ([FromBody]DeleteProductFromDailyLogByUserCommand command, ClaimsPrincipal user, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
            {
                var userId = user.GetUserIdByClaim();

                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }

                command.Dto.UserId = int.Parse(userId);
                
                return Results.Ok(await mediator.Send(command, cancellationToken));
            });
        }
    }
}
