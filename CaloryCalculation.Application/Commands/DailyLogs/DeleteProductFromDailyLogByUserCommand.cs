using CaloryCalculation.Application.DTOs.DailyLogs;
using MediatR;

namespace CaloryCalculation.Application.Commands.DailyLogs;

public record DeleteProductFromDailyLogByUserCommand(DeleteProductDTO Dto) : IRequest<bool>;