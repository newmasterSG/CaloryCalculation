using CaloryCalculation.Application.Commands.User;
using CaloryCalculation.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalculation.Application.Handlers.User;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly UserManager<CaloryCalculatiom.Domain.Entities.User> _userManager;

    public RegisterUserCommandHandler(UserManager<CaloryCalculatiom.Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request.register.Password != request.register.ConfirmPassword)
        {
            return new RegisterUserResult
            {
                Success = false,
                Errors = new List<string> { "Password and confirmation password do not match." }
            };
        }
        
        var existingUser = await _userManager.FindByEmailAsync(request.register.Email);
        if (existingUser != null)
        {
            return new RegisterUserResult
            {
                Success = false,
                Errors = new List<string> { "User with this email already exists." }
            };
        }
        
        var newUser = new CaloryCalculatiom.Domain.Entities.User()
        {
            FirstName = request.register.FirstName,
            LastName = request.register.LastName,
            Email = request.register.Email,
            UserName = request.register.Email,
            Height = request.register.Height,
            Weight = request.register.Weight,
            Age = request.register.Age,
            Gender = request.register.Gender
        };

        var result = await _userManager.CreateAsync(newUser, request.register.Password);

        if (!result.Succeeded)
        {
            return new RegisterUserResult
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        return new RegisterUserResult
        {
            Success = true,
            UserId = newUser.Id
        };
    }
}