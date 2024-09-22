using CaloryCalculatiom.Domain.Entities.Enums;
using CaloryCalculation.Application.Commands.DailyLogs;
using CaloryCalculation.Application.Interfaces;
using MediatR;

namespace CaloryCalculation.Application.Handlers.DailyLogs;

public class DeleteProductFromDailyLogByUserCommandHandler(IDailyLogService dailyLogService) : IRequestHandler<DeleteProductFromDailyLogByUserCommand, bool>
{
    public Task<bool> Handle(DeleteProductFromDailyLogByUserCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(MealType), (byte)request.Dto.MealType))
        {
            throw new ArgumentException("Invalid MealType value", nameof(request.Dto.MealType));
        }
        
        var mealType = (MealType)request.Dto.MealType;

        return dailyLogService.DeleteProductFromDailyLogByUserIdDateAsync((int)request.Dto.UserId!, request.Dto.ProductId,
            mealType, request.Dto.CreationDate, cancellationToken);
    }
}