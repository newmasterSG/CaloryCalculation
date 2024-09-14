using CaloryCalculation.Application.DTOs;
using MediatR;

namespace CaloryCalculation.Application.Commands.Token;

public record RefreshTokenCommand(RefreshTokenDTO token) : IRequest<RefreshTokenDTO>;