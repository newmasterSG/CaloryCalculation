using CaloryCalculation.Application.DTOs.DailyLogs;
using MediatR;

namespace CaloryCalculation.Application.Queries.DailyLog;

public record GetDailyLogByUserQuery(GetDailyLogUserDTO Dto) : IRequest<DailyLogDTO>;