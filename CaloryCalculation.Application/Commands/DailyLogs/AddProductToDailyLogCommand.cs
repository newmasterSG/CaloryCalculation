using CaloryCalculation.Application.DTOs.DailyLogs;
using MediatR;

namespace CaloryCalculation.Application.Commands.DailyLogs
{
    public record AddProductToDailyLogCommand(AddProductDailyLogDTO DTO) : IRequest
    {

    }
}
