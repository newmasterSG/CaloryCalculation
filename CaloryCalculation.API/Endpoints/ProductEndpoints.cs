using CaloryCalculation.Application.Commands.Product;
using CaloryCalculation.Application.DTOs.Products;
using CaloryCalculation.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CaloryCalculation.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/products");

            group.MapCreateProductEndpoint();
            group.MapDeleteProductEndpoint();
            group.MapGetProductEndpoint();
            group.MapGetProductsEndpoint();
            group.MapUpdateProductEndpoint();
        }

        private static void MapCreateProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromBody] CreateProductCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created($"/products/{result.Id}", result);
            });
        }

        private static void MapDeleteProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeleteProductCommand(id), cancellationToken);
                return result ? Results.NoContent() : Results.NotFound();
            });
        }

        private static void MapGetProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async ([FromRoute] int id, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetProductQuery(id), cancellationToken);
                return result != null ? Results.Ok(result) : Results.NotFound();
            });
        }

        private static void MapGetProductsEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async ([FromQuery] int page, [FromQuery] int pageSize, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                var get = new GetAllProduct
                {
                    Page = page,
                    PageSize = pageSize
                };

                var query = new GetProductsQuery(get);

                var results = await mediator.Send(query, cancellationToken);
                return Results.Ok(results);
            });
        }

        private static void MapUpdateProductEndpoint(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async ([FromRoute] int id, [FromBody] UpdateProductCommand command, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
            {
                command.DTO.Id = id;
                var result = await mediator.Send(command, cancellationToken);
                return result != null ? Results.Ok(result) : Results.NotFound();
            });
        }
    }
}
