using CaloryCalculation.Application.DTOs.User;
using MediatR;

namespace CaloryCalculation.Application.Commands.User;

public record LoginUserCommand(LoginUserDTO login) : IRequest<LoginUserResult> {}