using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Application.Options;
using CaloryCalculation.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CaloryCalculation.Application.Services;

public class TokenService (IOptions<JwtSettings> jwtSettings, IOptions<TokenProvider> tokenProvider, CaloryCalculationDbContext context, UserManager<User> userManager) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly TokenProvider _tokenProvider = tokenProvider.Value;

    public string GenerateAccessToken(string userId)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        
        ArgumentNullException.ThrowIfNull(user);

        bool isRevoked = await RevokeRefreshTokenAsync(user);
        
        var newRefreshToken = await userManager.GenerateUserTokenAsync(user, _tokenProvider.Name, ApplicationConstants.refreshTokenKeyDb);
        var res = await userManager.SetAuthenticationTokenAsync(user, _tokenProvider.Name, ApplicationConstants.refreshTokenKeyDb, newRefreshToken);

        return !res.Succeeded ? string.Empty : newRefreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        
        ArgumentNullException.ThrowIfNull(user);
        
        var refreshToken = await userManager.GetAuthenticationTokenAsync(user, _tokenProvider.Name, ApplicationConstants.refreshTokenKeyDb);
        var isValid = await userManager.VerifyUserTokenAsync(user, _tokenProvider.Name, ApplicationConstants.refreshTokenKeyDb, refreshToken );

        return isValid;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        return await RevokeRefreshTokenAsync(user);
    }
    
    public async Task<bool> RevokeRefreshTokenAsync(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var res = await userManager.RemoveAuthenticationTokenAsync(user, _tokenProvider.Name, ApplicationConstants.refreshTokenKeyDb);

        return res.Succeeded;
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateLifetime = false, // We are checking for an expired token here
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}