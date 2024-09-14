using System.Security.Claims;
using CaloryCalculation.Application.Commands.Token;
using CaloryCalculation.Application.DTOs;
using CaloryCalculation.Application.Interfaces;
using MediatR;

namespace CaloryCalculation.Application.Handlers.Token;

public class RefreshTokenCommandHandler(ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, RefreshTokenDTO>
{
    public async Task<RefreshTokenDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.token.AccessToken) && string.IsNullOrEmpty(request.token.RefreshToken))
        {
            return new RefreshTokenDTO();
        }

        
        var claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(request.token.AccessToken);
        
        var userIdClaim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        var userId = userIdClaim?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return new RefreshTokenDTO();
        }
        
        var isValid = await tokenService.ValidateRefreshTokenAsync(userId);

        if (!isValid)
        {
            return new RefreshTokenDTO();
        }
        
        var newAccessToken = tokenService.GenerateAccessToken(userId);

        var newRefreshToken = await tokenService.GenerateRefreshTokenAsync(userId);

        return new RefreshTokenDTO()
        {
            RefreshToken = newRefreshToken,
            AccessToken = newAccessToken
        };
    }
}