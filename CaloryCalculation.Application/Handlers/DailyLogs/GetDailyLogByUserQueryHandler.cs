using CaloryCalculation.Application.DTOs.DailyLogs;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Application.Queries.DailyLog;
using MediatR;

namespace CaloryCalculation.Application.Handlers.DailyLogs;

public class GetDailyLogByUserQueryHandler(IDailyLogService dailyLogService) : IRequestHandler<GetDailyLogByUserQuery, DailyLogDTO>
{
    public async Task<DailyLogDTO> Handle(GetDailyLogByUserQuery request, CancellationToken cancellationToken)
    {
        return await dailyLogService.GetDailyLogForUserAsync(request.Dto);
    }
}