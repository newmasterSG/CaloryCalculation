using System.Security.Claims;
using CaloryCalculatiom.Domain.Entities;

namespace CaloryCalculation.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(string userId);
    Task<string> GenerateRefreshTokenAsync(string userId);
    Task<bool> ValidateRefreshTokenAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(string userId);
    Task<bool> RevokeRefreshTokenAsync(User user);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}