using CaloryCalculation.Application.DTOs.User;
using MediatR;

namespace CaloryCalculation.Application.Commands.User;

public record RegisterUserCommand(RegisterUserDTO register) : IRequest<RegisterUserResult> {}