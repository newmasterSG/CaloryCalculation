using CaloryCalculation.Application.DTOs.User;
using MediatR;

namespace CaloryCalculation.Application.Commands.User;

public record UpdateUserCommand(UserProfileUpdateDto upd) : IRequest<bool>;