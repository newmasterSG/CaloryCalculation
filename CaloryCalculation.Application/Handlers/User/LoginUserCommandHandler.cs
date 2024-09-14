using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.DTOs.User;
using CaloryCalculation.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalculation.Application.Handlers.User;

public class LoginUserCommandHandler(
    SignInManager<CaloryCalculatiom.Domain.Entities.User> signInManager,
    UserManager<CaloryCalculatiom.Domain.Entities.User> userManager,
    ITokenService tokenService) : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.login.Email);
        if (user == null)
        {
            return new LoginUserResult
            {
                Success = false,
            };
        }
        
        var result = await signInManager.CheckPasswordSignInAsync(user, request.login.Password, false);
        if (!result.Succeeded)
        {
            return new LoginUserResult
            {
                Success = false,
            };
        }
        
        var accessToken =  tokenService.GenerateAccessToken(user.Id.ToString());
        var refreshToken = await tokenService.GenerateRefreshTokenAsync(user.Id.ToString());
        
        return new LoginUserResult
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}