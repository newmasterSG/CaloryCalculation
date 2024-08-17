using CaloryCalculation.Application.Commands.DailyLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalculation.API.Endpoints
{
    public static class DailyLogEndpoints
    {
        public static IEndpointRouteBuilder MapDailyLogEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/dailylog");

            group.MapPostDailyLogEndpoint();

            return routes;
        }

        private static void MapPostDailyLogEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/add-product", async ([FromBody]AddProductToDailyLogCommand command, [FromServices]IMediator mediator, CancellationToken cancellationToken) =>
            {
                await mediator.Send(command, cancellationToken);
                return Results.Ok();
            });
        }
    }
}
