
using CaloryCalculation.Application.Commands.Token;
using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.DTOs.Token;
using CaloryCalculation.Application.DTOs.User;
using CaloryCalculation.Application.Mappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace CaloryCalculation.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/auth");

        group.MapRegisterEndpoint();
        group.MapLoginEndpoint();
        
        group.MapRefreshTokenEndpoint();
        return routes;
    }
    
    private static void MapRegisterEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/register", async ([FromBody] RegisterUserDTO registerUserDTO, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = registerUserDTO.ToCommand();
            
            var result = await mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Created($"/auth/register/{result.UserId}", result);
        });
    }

    private static void MapLoginEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async ([FromBody] LoginUserDTO login, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(login);
            
            var result = await mediator.Send(command, cancellationToken);

            if (!result.Success)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            });
        });
    }
    
    private static void MapRefreshTokenEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/refresh-token", async ([FromBody] RefreshTokenDTO tokenDto, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommand(tokenDto);
            
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(new
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            });
        });
    }
}